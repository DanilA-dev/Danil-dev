#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace D_Dev
{
    public static class EditorTools
    {
        private static ListRequest _listRequest;
        
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
            var path = "Assets";
            var directory = Directory.GetCurrentDirectory() + "/D-Dev_Utils.unitypackage";
            AssetDatabase.ExportPackage(path, directory, ExportPackageOptions.Recurse);
            Debug.Log($"Exported package to {directory}");
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
