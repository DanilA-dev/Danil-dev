using System;
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
        private const string LocalPackageDirectory = "Assets/Danil-dev/Package/";
        private const string VersionPrefsKey = "D_Dev_InstalledVersion_";
        private const string ImportQueueKey = "D_Dev_ImportQueue_";
        private const string DismissedVersionKey = "D_Dev_DismissedVersion_";
        private const string DialogShownSessionKey = "D_Dev_InstallDialogShown";
        private const string MainPackageName = "Danil-Dev";
        private const string BasePackageName = MainPackageName + ".Base";
        private const string AssetsPackageName = MainPackageName + ".Assets";
        private const string PluginsPackageName = MainPackageName + ".plugins";
        private const string PackageExtension = ".unitypackage";
        private const string GuidsExtension = ".guids.txt";
        private const char QueueSeparator = ';';

        // Plugins keep their own generated settings (e.g. Odin config), so obsolete cleanup is disabled for them
        private static readonly (string Name, string Folder, bool RemoveObsolete)[] PackageParts =
        {
            (PluginsPackageName, "Assets/Danil-dev/Plugins", false),
            (BasePackageName, "Assets/Danil-dev/Scripts", true),
            (AssetsPackageName, "Assets/Danil-dev/Assets", true)
        };

        private static readonly Dictionary<string, string> GitPackages = new()
        {
            {"com.cysharp.unitask", "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask"},
        };

        #endregion

        #region Auto Setup

        static EditorTools()
        {
            if (GetImportQueue().Length > 0)
            {
                EditorApplication.delayCall += ContinueImportQueue;
                return;
            }

            if (IsDevProject())
                return;

            var projectHash = GetProjectHash();
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
            AddGitPackagesToManifest();
            SetImportQueue(PackageParts.Select(part => part.Name));
            ImportNextInQueue();
        }

        [MenuItem("Tools/D_Dev/Setup/Import/Import Dependencies")]
        public static void InstallDependencies()
        {
            AddGitPackagesToManifest();
            StartImport(PluginsPackageName);
        }

        private static void AddGitPackagesToManifest()
        {
            foreach (var (packageName, packageURL) in GitPackages)
                AddPackageToManifest(packageName, packageURL);

            Debug.Log("[D-Dev] Manifest dependencies added");
        }

        [MenuItem("Tools/D_Dev/Setup/Import/Import Base")]
        public static void ImportBase() => StartImport(BasePackageName);

        [MenuItem("Tools/D_Dev/Setup/Import/Import Assets")]
        public static void ImportAssets() => StartImport(AssetsPackageName);

        private static void StartImport(string packageName)
        {
            SetImportQueue(new[] { packageName });
            ImportNextInQueue();
        }

        private static void ContinueImportQueue()
        {
            if (EditorApplication.isCompiling)
            {
                EditorApplication.update -= WaitForCompilationThenImport;
                EditorApplication.update += WaitForCompilationThenImport;
                return;
            }

            ImportNextInQueue();
        }

        private static void WaitForCompilationThenImport()
        {
            if (EditorApplication.isCompiling)
                return;

            EditorApplication.update -= WaitForCompilationThenImport;
            ImportNextInQueue();
        }

        private static void ImportNextInQueue()
        {
            var queue = GetImportQueue();
            if (queue.Length == 0)
                return;

            var packageName = queue[0];
            SetImportQueue(queue.Skip(1));

            var packagePath = ResolvePackagePath(packageName + PackageExtension);
            if (packagePath == null)
            {
                Debug.LogError($"[D-Dev] {packageName}{PackageExtension} not found");
                ImportNextInQueue();
                return;
            }

            UnsubscribeFromImport();
            AssetDatabase.importPackageCompleted += OnPackageImported;
            AssetDatabase.importPackageCancelled += OnPackageCancelled;
            AssetDatabase.importPackageFailed += OnPackageFailed;
            AssetDatabase.ImportPackage(packagePath, true);
        }

        private static void OnPackageImported(string packageName)
        {
            if (!IsPackagePart(packageName))
                return;

            UnsubscribeFromImport();
            RemoveObsoleteAssets(packageName);

            if (GetImportQueue().Length > 0)
            {
                EditorApplication.delayCall += ContinueImportQueue;
                return;
            }

            if (packageName == PluginsPackageName)
            {
                Debug.Log("[D-Dev] Dependencies installed");
                return;
            }

            EditorPrefs.SetString(VersionPrefsKey + GetProjectHash(), GetPackageVersion());
            Debug.Log("[D-Dev] Setup complete");
        }

        private static void OnPackageCancelled(string packageName)
        {
            if (!IsPackagePart(packageName))
                return;

            UnsubscribeFromImport();
            ClearImportQueue();
            Debug.Log($"[D-Dev] Import of {packageName} cancelled, nothing was changed");
        }

        private static void OnPackageFailed(string packageName, string errorMessage)
        {
            if (!IsPackagePart(packageName))
                return;

            UnsubscribeFromImport();
            ClearImportQueue();
            Debug.LogError($"[D-Dev] Import of {packageName} failed: {errorMessage}");
        }

        private static void UnsubscribeFromImport()
        {
            AssetDatabase.importPackageCompleted -= OnPackageImported;
            AssetDatabase.importPackageCancelled -= OnPackageCancelled;
            AssetDatabase.importPackageFailed -= OnPackageFailed;
        }

        private static void RemoveObsoleteAssets(string packageName)
        {
            var part = PackageParts.FirstOrDefault(p => p.Name == packageName);
            if (!part.RemoveObsolete)
                return;

            var folder = part.Folder;
            if (folder == null || !AssetDatabase.IsValidFolder(folder))
                return;

            var guidsFile = packageName + GuidsExtension;
            var guidsPath = ResolvePackagePath(guidsFile);
            if (guidsPath == null)
            {
                Debug.LogWarning($"[D-Dev] {guidsFile} not found, obsolete assets were not removed");
                return;
            }

            var packageGuids = new HashSet<string>(File.ReadAllLines(guidsPath)
                .Select(line => line.Trim())
                .Where(line => line.Length > 0));

            var obsoletePaths = AssetDatabase.FindAssets("", new[] { folder })
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
            var projectHash = GetProjectHash();
            EditorPrefs.DeleteKey(VersionPrefsKey + projectHash);
            EditorPrefs.DeleteKey(ImportQueueKey + projectHash);
            EditorPrefs.DeleteKey(DismissedVersionKey + projectHash);
            SessionState.EraseBool(DialogShownSessionKey);
            Debug.Log("[D-Dev] Install state reset — dialog will appear on next domain reload");
        }

        public static void DismissCurrentVersion()
        {
            EditorPrefs.SetString(DismissedVersionKey + GetProjectHash(), GetPackageVersion());
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

            foreach (var part in PackageParts.Where(part => part.Name != PluginsPackageName))
                ExportPart(part);

            AssetDatabase.Refresh();
            Debug.Log($"[D-Dev] Exported package v{GetPackageVersion()}");
        }

        [MenuItem("Tools/D_Dev/Setup/Export Plugins")]
        public static void ExportPlugins()
        {
            BumpPatchVersion();
            ExportPart(PackageParts.First(part => part.Name == PluginsPackageName));

            AssetDatabase.Refresh();
            Debug.Log($"[D-Dev] Exported plugins v{GetPackageVersion()}");
        }

        private static void ExportPart((string Name, string Folder, bool RemoveObsolete) part)
        {
            if (!AssetDatabase.IsValidFolder(part.Folder))
            {
                Debug.LogWarning($"[D-Dev] {part.Folder} not found, {part.Name} was not exported");
                return;
            }

            var exportPath = LocalPackageDirectory + part.Name + PackageExtension;
            AssetDatabase.ExportPackage(part.Folder, exportPath, ExportPackageOptions.Recurse);

            var guids = AssetDatabase.FindAssets("", new[] { part.Folder });
            if (part.RemoveObsolete)
                File.WriteAllLines(LocalPackageDirectory + part.Name + GuidsExtension, guids);

            Debug.Log($"[D-Dev] Exported {exportPath} ({guids.Length} assets)");
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

        private static string GetProjectHash() => Application.dataPath.GetHashCode().ToString();

        private static bool IsPackagePart(string packageName) => GetPackageFolder(packageName) != null;

        private static string GetPackageFolder(string packageName) =>
            PackageParts.FirstOrDefault(part => part.Name == packageName).Folder;

        private static string[] GetImportQueue()
        {
            var value = EditorPrefs.GetString(ImportQueueKey + GetProjectHash(), "");
            return value.Split(new[] { QueueSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .Where(IsPackagePart)
                .ToArray();
        }

        private static void SetImportQueue(IEnumerable<string> packageNames)
        {
            var value = string.Join(QueueSeparator.ToString(), packageNames);
            if (string.IsNullOrEmpty(value))
            {
                ClearImportQueue();
                return;
            }

            EditorPrefs.SetString(ImportQueueKey + GetProjectHash(), value);
        }

        private static void ClearImportQueue() => EditorPrefs.DeleteKey(ImportQueueKey + GetProjectHash());

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
            window.minSize = new Vector2(420, 180);
            window.maxSize = new Vector2(420, 180);
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
                if (GUILayout.Button("Import Dependencies", GUILayout.Height(22)))
                {
                    Close();
                    EditorTools.InstallDependencies();
                    return;
                }

                if (GUILayout.Button("Import Base", GUILayout.Height(22)))
                {
                    Close();
                    EditorTools.ImportBase();
                    return;
                }

                if (GUILayout.Button("Import Assets", GUILayout.Height(22)))
                {
                    Close();
                    EditorTools.ImportAssets();
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
