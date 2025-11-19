using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace D_Dev.SceneLoader
{
    [CreateAssetMenu(menuName = "D-Dev/ProjectScenesConfig")]
    public class ProjectScenesConfig : ScriptableObject
    {
        #region Fields

        [SerializeField] private List<SceneInfo> _scenes = new();

        #endregion

        #region Properties

        public List<SceneInfo> Scenes => _scenes;

        #endregion

#if UNITY_EDITOR
        #region Editor

        [Button]
        private void FindSceneInfos()
        {
            var sceneInfosGuids = AssetDatabase.FindAssets("t:SceneInfo");
            foreach (var guid in sceneInfosGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var sceneInfo = AssetDatabase.LoadAssetAtPath<SceneInfo>(path);
                if (sceneInfo != null && !_scenes.Contains(sceneInfo))
                    _scenes.Add(sceneInfo);
            }
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
        
        [MenuItem("Tools/D_Dev/Project Scenes Config")]
        private static void OpenScenesConfig()
        {
            ProjectScenesConfig projectScenesConfig = Resources.Load<ProjectScenesConfig>("ProjectScenesConfig");
            var resourcesDirFullPath = Path.Combine(Application.dataPath, "_Project", "Resources");
            var assetRelativePath = "Assets/_Project/Resources/ProjectScenesConfig.asset";

            if (projectScenesConfig == null)
            {
                if (!Directory.Exists(resourcesDirFullPath))
                    Directory.CreateDirectory(resourcesDirFullPath);

                projectScenesConfig = ScriptableObject.CreateInstance<ProjectScenesConfig>();
                AssetDatabase.CreateAsset(projectScenesConfig, assetRelativePath);
                AssetDatabase.Refresh();
            }
            EditorUtility.OpenPropertyEditor(projectScenesConfig);
        }

        #endregion
    }
#endif
}
