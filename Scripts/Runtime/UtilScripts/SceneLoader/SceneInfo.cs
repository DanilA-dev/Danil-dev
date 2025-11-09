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


        #endregion

        #region Properties

        public string SceneName => _sceneName;

        #endregion
#if UNITY_EDITOR
        #region Editor

        
        private void GetSceneName()
        {
            if (string.IsNullOrEmpty(_sceneName) && sceneAsset != null)
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