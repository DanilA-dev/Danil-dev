#if UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace D_Dev
{
    public static class EditorTools
    {
        #region Const

        private static readonly string UniTaskPackage = "com.cysharp.unitask";

        private static readonly string UniTaskURL =
            "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask";

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
                "Assets/Danil-dev/Plugins",
                "Assets/Danil-dev/Scripts"
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
            AddPackageToManifest(UniTaskPackage, UniTaskURL);
        }

        [MenuItem("Tools/D_Dev/Open PersistentDataPath")]
        public static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
        
        [MenuItem("Tools/D_Dev/Clear All Data")]
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
            var addedPackagePath = text.Replace("\"dependencies\": {", "\"dependencies\": {" + Environment.NewLine + " " +
                                                                       '"' + package + '"'+ ":" + " " + '"' + url + '"' + ",");
            
            File.WriteAllText(manifest, addedPackagePath, Encoding.Default);
        }

        #endregion
    }
}
#endif
