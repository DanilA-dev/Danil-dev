#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace D_Dev
{
    [InitializeOnLoad]
    public static class EditorTools
    {
        #region Const

        private static readonly Dictionary<string, string> CustomPackages = new()
        {
            {"com.cysharp.unitask", "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask"},
        };

        private const string BootstrapKey = "DDevBootstrapCompleted";

        #endregion

        #region Bootstrap

        private static ListRequest _listRequest;
        private static AddRequest _addRequest;

        static EditorTools()
        {
            if (SessionState.GetBool(BootstrapKey, false))
                return;

            SessionState.SetBool(BootstrapKey, true);
            CheckDependencies();
        }

        private static void CheckDependencies()
        {
            _listRequest = Client.List(true);
            EditorApplication.update += OnListRequestComplete;
        }

        private static void OnListRequestComplete()
        {
            if (!_listRequest.IsCompleted)
                return;

            EditorApplication.update -= OnListRequestComplete;

            if (_listRequest.Status == StatusCode.Failure)
            {
                Debug.LogError("[D-Dev] Failed to list packages: " + _listRequest.Error.message);
                return;
            }

            var installedPackages = new HashSet<string>();
            foreach (var package in _listRequest.Result)
                installedPackages.Add(package.name);

            foreach (var (packageName, packageUrl) in CustomPackages)
            {
                if (!installedPackages.Contains(packageName))
                {
                    Debug.Log($"[D-Dev] {packageName} not found. Installing automatically...");
                    _addRequest = Client.Add(packageUrl);
                    EditorApplication.update += OnAddRequestComplete;
                }
            }

#if !ODIN_INSPECTOR
            Debug.LogWarning("[D-Dev] Odin Inspector not found. Most inspector features will be disabled. Please install Odin Inspector.");
#endif
#if !DOTWEEN
            Debug.LogWarning("[D-Dev] DOTween not found. Tween systems will be disabled. Please install DOTween and run its setup.");
#endif
        }

        private static void OnAddRequestComplete()
        {
            if (!_addRequest.IsCompleted)
                return;

            EditorApplication.update -= OnAddRequestComplete;

            if (_addRequest.Status == StatusCode.Failure)
                Debug.LogError("[D-Dev] Failed to install package: " + _addRequest.Error.message);
            else
                Debug.Log($"[D-Dev] Package installed successfully: {_addRequest.Result.displayName}");
        }

        #endregion

        #region Editor

        [MenuItem("Tools/D_Dev/Setup/Create Folders")]
        public static void InitProjectFolders()
        {
            CreateFolders("_Project" ,new []
            {
                "Art",
                "Animations",
                "Audio",
                "Scripts",
                "Scenes",
                "Resources",
                "Prefabs",
                "ScriptableObjects"
            });
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/D_Dev/Setup/Export Package")]
        public static void ExportPackage()
        {
            string[] paths = new []
            {
                "Assets/Danil-dev/Assets",
            } ;
            var exportDirectory =  "Assets/Danil-dev/Package/Danil-Dev.unitypackage";
            AssetDatabase.ExportPackage(paths, exportDirectory, ExportPackageOptions.Recurse);
            Debug.Log($"Exported package to {exportDirectory}");
        }

        [MenuItem("Tools/D_Dev/Setup/Import Package")]
        public static void ImportUtilsPackage()
        {
            var path = "Packages/com.d-dev.utils/Danil-Dev.unitypackage";
            AssetDatabase.ImportPackage(path,true);
        }
        
        [MenuItem("Tools/D_Dev/Setup/Import Dependencies")]
        public static void ImportDependencies()
        {
            foreach (var (packageName, packageURL) in CustomPackages)
                AddPackageToManifest(packageName, packageURL);
            
            var path = "Packages/com.d-dev.utils/Danil-Dev.plugins.unitypackage";
            AssetDatabase.ImportPackage(path,true);
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/D_Dev/Data/Open PersistentDataPath")]
        public static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
        
        [MenuItem("Tools/D_Dev/Data/Clear All Data")]
        public static void ClearData()
        {
            PlayerPrefs.DeleteAll();

            DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
            foreach (FileInfo file in dir.GetFiles())
                file.Delete();
            foreach (DirectoryInfo subDir in dir.GetDirectories())
                subDir.Delete(true);
            
            Debug.Log("All data cleared");
        }
        #endregion

        #region Helpers
        private static void CreateFolders(string rootDir, string[] directions)
        {
            var path = Application.dataPath;
            var combinedPath = Path.Combine(path, rootDir);
            
            foreach (var dir in directions)
                Directory.CreateDirectory(Path.Combine(combinedPath, dir));
        }
        
        private static void AddPackageToManifest(string package, string url)
        {
            var manifest = "Packages/manifest.json";
            var text = File.ReadAllText(manifest, Encoding.Default);
            if(text.Contains(package) || text.Contains(url))
                return;

            var newPackageLine = ",\n    \"" + package + "\": \"" + url + "\"";
            var addedPackagePath = text.Replace("\n  }\n}", newPackageLine + "\n  }\n}");
            File.WriteAllText(manifest, addedPackagePath, Encoding.Default);
            AssetDatabase.Refresh();
        }

        #endregion
    }
}
#endif
