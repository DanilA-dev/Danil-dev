using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace D_Dev.SceneLoader
{
    [CreateAssetMenu(menuName = "D-Dev/Info/SceneInfo")]
    public class SceneInfo : ScriptableObject
    {
        #region Fields

        #if UNITY_EDITOR
        [SerializeField, OnValueChanged(nameof(GetSceneName))] private SceneAsset sceneAsset;
        #endif
        [SerializeField, ReadOnly] private string _sceneName;
        [SerializeField] private bool _addSceneOnStartup;
        [SerializeField] private bool _isUnloadable;

        #endregion

        #region Properties

        public string SceneName => _sceneName;
        public bool IsUnloadable => _isUnloadable;

        public bool AddSceneOnStartup => _addSceneOnStartup;

        #endregion
#if UNITY_EDITOR
        #region Editor

        [Button("Update Scene Name")]
        private void GetSceneName()
        {
            if (sceneAsset != null)
            {
                _sceneName = sceneAsset.name;
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
            }
        }

        #endregion
#endif
    }
}