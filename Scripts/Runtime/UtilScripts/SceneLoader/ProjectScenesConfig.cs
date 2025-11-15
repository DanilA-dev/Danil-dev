using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

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

        #endregion
    }
#endif
}
