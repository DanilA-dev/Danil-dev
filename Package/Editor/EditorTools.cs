using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace D_Dev
{
    [InitializeOnLoad]
    public static class EditorTools
    {
        #region Const

        private const string PackagePath = "Packages/com.d-dev.utils/";
        private const string VersionPrefsKey = "D_Dev_InstalledVersion_";
        private const string InstallStateKey = "D_Dev_InstallState_";
        private const string DismissedVersionKey = "D_Dev_DismissedVersion_";
        private const string DialogShownSessionKey = "D_Dev_InstallDialogShown";
        private const string MainPackageName = "Danil-Dev";
        private const string MainPackageFile = MainPackageName + ".unitypackage";
        private const string PackageGuidsFile = MainPackageName + ".guids.txt";

        private static readonly string[] PackageFolders =
        {
            "Assets/Danil-dev/Assets",
            "Assets/Danil-dev/Scripts"
        };

        private static readonly Dictionary<string, string> GitPackages = new()
        {
            {"com.cysharp.unitask", "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask"},
        };

        #endregion

        #region Auto Setup

        static EditorTools()
        {
            var projectHash = Application.dataPath.GetHashCode().ToString();
            var stateKey = InstallStateKey + projectHash;
            var state = EditorPrefs.GetString(stateKey, "");

            if (state == "plugins_done")
            {
                EditorApplication.delayCall += () =>
                {
                    if (EditorApplication.isCompiling)
                    {
                        EditorApplication.update += WaitForCompilationThenImport;
                        return;
                    }
                    ImportMainPackage();
                };
                return;
            }

            if (IsDevProject())
                return;

            var packageVersion = GetPackageVersion();
            var versionKey = VersionPrefsKey + projectHash;
            var installedVersion = EditorPrefs.GetString(versionKey, "");

            if (installedVersion == packageVersion)
                return;

            if (EditorPrefs.GetString(DismissedVersionKey + projectHash, "") == packageVersion)
                return;

            if (SessionState.GetBool(DialogShownSessionKey, false))
                return;

            SessionState.SetBool(DialogShownSessionKey, true);

            var isUpdate = !string.IsNullOrEmpty(installedVersion);
            var title = isUpdate ? "D-Dev Utils — Update" : "D-Dev Utils";
            var message = isUpdate
                ? $"Update available: {installedVersion} → {packageVersion}\n\nReimport Scripts & Assets?"
                : "Install D-Dev Utils package?";

            EditorApplication.delayCall += () => InstallDialog.Show(title, message);
        }

        private static bool IsDevProject()
        {
            var localPackage = Path.Combine(Application.dataPath, "Danil-dev/Package/package.json");
            return File.Exists(localPackage);
        }

        public static void InstallAll()
        {
            InstallDependencies();

            var projectHash = Application.dataPath.GetHashCode().ToString();
            var stateKey = InstallStateKey + projectHash;

            var pluginsPath = ResolvePackagePath("Danil-Dev.plugins.unitypackage");
            if (pluginsPath != null)
            {
                EditorPrefs.SetString(stateKey, "plugins_done");
                AssetDatabase.ImportPackage(pluginsPath, true);
                return;
            }

            InstallPackage();
        }

        [MenuItem("Tools/D_Dev/Setup/Install Dependencies")]
        public static void InstallDependencies()
        {
            foreach (var (packageName, packageURL) in GitPackages)
                AddPackageToManifest(packageName, packageURL);

            Debug.Log("[D-Dev] Dependencies installed");
        }

        [MenuItem("Tools/D_Dev/Setup/Install Package")]
        public static void InstallPackage() => ImportMainPackage();

        private static void WaitForCompilationThenImport()
        {
            if (EditorApplication.isCompiling)
                return;

            EditorApplication.update -= WaitForCompilationThenImport;
            ImportMainPackage();
        }

        private static void ImportMainPackage()
        {
            var projectHash = Application.dataPath.GetHashCode().ToString();

            EditorPrefs.DeleteKey(InstallStateKey + projectHash);

            var mainPath = ResolvePackagePath(MainPackageFile);
            if (mainPath == null)
            {
                Debug.LogError($"[D-Dev] {MainPackageFile} not found");
                return;
            }

            UnsubscribeFromImport();
            AssetDatabase.importPackageCompleted += OnMainPackageImported;
            AssetDatabase.importPackageCancelled += OnMainPackageCancelled;
            AssetDatabase.importPackageFailed += OnMainPackageFailed;
            AssetDatabase.ImportPackage(mainPath, true);
        }

        private static void OnMainPackageImported(string packageName)
        {
            if (packageName != MainPackageName)
                return;

            UnsubscribeFromImport();
            RemoveObsoleteAssets();

            var projectHash = Application.dataPath.GetHashCode().ToString();
            EditorPrefs.SetString(VersionPrefsKey + projectHash, GetPackageVersion());
            Debug.Log("[D-Dev] Setup complete");
        }

        private static void OnMainPackageCancelled(string packageName)
        {
            if (packageName != MainPackageName)
                return;

            UnsubscribeFromImport();
            Debug.Log("[D-Dev] Import cancelled, nothing was changed");
        }

        private static void OnMainPackageFailed(string packageName, string errorMessage)
        {
            if (packageName != MainPackageName)
                return;

            UnsubscribeFromImport();
            Debug.LogError($"[D-Dev] Import failed: {errorMessage}");
        }

        private static void UnsubscribeFromImport()
        {
            AssetDatabase.importPackageCompleted -= OnMainPackageImported;
            AssetDatabase.importPackageCancelled -= OnMainPackageCancelled;
            AssetDatabase.importPackageFailed -= OnMainPackageFailed;
        }

        private static void RemoveObsoleteAssets()
        {
            var guidsPath = ResolvePackagePath(PackageGuidsFile);
            if (guidsPath == null)
            {
                Debug.LogWarning($"[D-Dev] {PackageGuidsFile} not found, obsolete assets were not removed");
                return;
            }

            var packageGuids = new HashSet<string>(File.ReadAllLines(guidsPath)
                .Select(line => line.Trim())
                .Where(line => line.Length > 0));

            var folders = PackageFolders.Where(AssetDatabase.IsValidFolder).ToArray();
            if (folders.Length == 0)
                return;

            var obsoletePaths = AssetDatabase.FindAssets("", folders)
                .Where(guid => !packageGuids.Contains(guid))
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => !string.IsNullOrEmpty(path))
                .Distinct()
                .OrderByDescending(path => path.Length)
                .ToArray();

            if (obsoletePaths.Length == 0)
                return;

            var failedPaths = new List<string>();
            AssetDatabase.DeleteAssets(obsoletePaths, failedPaths);

            foreach (var path in obsoletePaths.Except(failedPaths))
                Debug.Log($"[D-Dev] Removed obsolete {path}");

            foreach (var path in failedPaths)
                Debug.LogWarning($"[D-Dev] Failed to remove obsolete {path}");
        }

        #endregion

        #region Editor

        [MenuItem("Tools/D_Dev/Setup/Install All")]
        public static void MenuInstallAll() => InstallAll();

        [MenuItem("Tools/D_Dev/Setup/Reset Install State")]
        public static void ResetInstallState()
        {
            var projectHash = Application.dataPath.GetHashCode().ToString();
            EditorPrefs.DeleteKey(VersionPrefsKey + projectHash);
            EditorPrefs.DeleteKey(InstallStateKey + projectHash);
            EditorPrefs.DeleteKey(DismissedVersionKey + projectHash);
            SessionState.EraseBool(DialogShownSessionKey);
            Debug.Log("[D-Dev] Install state reset — dialog will appear on next domain reload");
        }

        public static void DismissCurrentVersion()
        {
            var projectHash = Application.dataPath.GetHashCode().ToString();
            EditorPrefs.SetString(DismissedVersionKey + projectHash, GetPackageVersion());
        }

        [MenuItem("Tools/D_Dev/Setup/Create Folders")]
        public static void InitProjectFolders()
        {
            CreateFolders("_Project", new[]
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
            BumpPatchVersion();

            var exportDirectory = "Assets/Danil-dev/Package/" + MainPackageFile;
            AssetDatabase.ExportPackage(PackageFolders, exportDirectory, ExportPackageOptions.Recurse);

            var guids = AssetDatabase.FindAssets("", PackageFolders);
            File.WriteAllLines("Assets/Danil-dev/Package/" + PackageGuidsFile, guids);
            AssetDatabase.Refresh();

            Debug.Log($"[D-Dev] Exported package v{GetPackageVersion()} to {exportDirectory} ({guids.Length} assets)");
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

        private static string ResolvePackagePath(string relativePath)
        {
            var localPath = Path.Combine(Application.dataPath, "Danil-dev/Package", relativePath);
            if (File.Exists(localPath))
                return localPath;

            var upmPath = Path.GetFullPath(Path.Combine(PackagePath, relativePath));
            return File.Exists(upmPath) ? upmPath : null;
        }

        private static string GetPackageVersion()
        {
            var path = ResolvePackagePath("package.json");
            if (path == null)
                return "0.0.0";

            var json = File.ReadAllText(path);
            var match = Regex.Match(json, "\"version\"\\s*:\\s*\"([^\"]+)\"");
            return match.Success ? match.Groups[1].Value : "0.0.0";
        }

        private static void BumpPatchVersion()
        {
            var path = ResolvePackagePath("package.json");
            if (path == null)
                return;

            var json = File.ReadAllText(path, Encoding.Default);
            var match = Regex.Match(json, "\"version\"\\s*:\\s*\"(\\d+)\\.(\\d+)\\.(\\d+)\"");
            if (!match.Success)
                return;

            var major = match.Groups[1].Value;
            var minor = match.Groups[2].Value;
            var patch = int.Parse(match.Groups[3].Value) + 1;
            var newVersion = $"{major}.{minor}.{patch}";

            json = json.Replace(match.Value, $"\"version\": \"{newVersion}\"");
            File.WriteAllText(path, json, Encoding.Default);
            Debug.Log($"[D-Dev] Version bumped to {newVersion}");
        }

        private static void AddPackageToManifest(string package, string url)
        {
            var manifest = "Packages/manifest.json";
            var text = File.ReadAllText(manifest, Encoding.Default);
            if (text.Contains(package) || text.Contains(url))
                return;

            var newPackageLine = ",\n    \"" + package + "\": \"" + url + "\"";
            var addedPackagePath = text.Replace("\n  }\n}", newPackageLine + "\n  }\n}");
            File.WriteAllText(manifest, addedPackagePath, Encoding.Default);
            AssetDatabase.Refresh();
        }

        #endregion
    }

    public class InstallDialog : EditorWindow
    {
        private string _message;

        public static void Show(string title, string message)
        {
            var window = CreateInstance<InstallDialog>();
            window.titleContent = new GUIContent(title);
            window._message = message;
            window.minSize = new Vector2(380, 180);
            window.maxSize = new Vector2(380, 180);
            window.ShowUtility();
        }

        private void OnGUI()
        {
            GUILayout.Space(12);
            EditorGUILayout.LabelField(_message, EditorStyles.wordWrappedLabel);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Install All", GUILayout.Height(26)))
            {
                Close();
                EditorTools.InstallAll();
                return;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Install Dependencies", GUILayout.Height(22)))
                {
                    Close();
                    EditorTools.InstallDependencies();
                    return;
                }

                if (GUILayout.Button("Install Package", GUILayout.Height(22)))
                {
                    Close();
                    EditorTools.InstallPackage();
                    return;
                }
            }

            if (GUILayout.Button("Skip This Version", GUILayout.Height(22)))
            {
                Close();
                EditorTools.DismissCurrentVersion();
            }

            GUILayout.Space(6);
        }
    }
}
