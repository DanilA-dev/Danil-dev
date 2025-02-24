#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace D_Dev
{
    public static class EditorTools
    {
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
                "ScriptableObjects"
            });
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/D_Dev/ExportPackage")]
        public static void ExportPackage()
        {
            var path =  "Assets/Danil-dev";
            var exportDirectory =  "Assets/Danil-dev/UnityPackages/Danil-Dev.unitypackage";
            AssetDatabase.ExportPackage(path, exportDirectory, ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
            Debug.Log($"Exported package to {exportDirectory}");
        }

        [MenuItem("Tools/D_Dev/Setup/ImportPackage")]
        public static void ImportUtilsPackage()
        {
            var path = "Packages/com.d-dev.utils/Danil-Dev.unitypackage";
            AssetDatabase.ImportPackage(path,true);
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

        #endregion
    }
}
#endif
