using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
#endif

namespace D_Dev.SceneLoader
{
    [System.Serializable]
    public class AddressablesSceneLoadData
    {
        #region Fields

        [OnValueChanged(nameof(OnUseAddressablesChanged))]
        [SerializeField] private bool _makeAddressable;

        [HideIf(nameof(_makeAddressable))]
        [SerializeField] private string _sceneName;

        [ShowIf(nameof(_makeAddressable))]
        [SerializeField] private AssetReference _sceneReference;

        private AsyncOperationHandle<SceneInstance>? _loadHandle;

        #endregion

        #region Properties

        public bool IsAddressable => _makeAddressable;
        public string SceneName => _sceneName;
        public AssetReference SceneReference => _sceneReference;
        public bool IsLoaded => _loadHandle.HasValue && _loadHandle.Value.IsValid()
                                && _loadHandle.Value.Result.Scene.isLoaded;

        #endregion

        #region Public

        public async UniTask LoadAsync(LoadSceneMode mode = LoadSceneMode.Additive)
        {
            if (!_makeAddressable)
            {
                await SceneManager.LoadSceneAsync(_sceneName, mode);
                return;
            }

            if (_sceneReference == null || !_sceneReference.RuntimeKeyIsValid())
            {
                Debug.LogError($"[AddressablesSceneLoadData] SceneReference is not set or invalid for '{_sceneName}'");
                return;
            }

            if (_loadHandle.HasValue && _loadHandle.Value.IsValid())
            {
                Debug.LogWarning($"[AddressablesSceneLoadData] Scene '{_sceneName}' is already loaded");
                return;
            }

            _loadHandle = Addressables.LoadSceneAsync(_sceneReference, mode);
            await _loadHandle.Value.ToUniTask();

            _sceneName = _loadHandle.Value.Result.Scene.name;
        }

        public async UniTask UnloadAsync()
        {
            if (!_makeAddressable)
            {
                await SceneManager.UnloadSceneAsync(_sceneName);
                return;
            }

            if (!_loadHandle.HasValue || !_loadHandle.Value.IsValid())
            {
                Debug.LogWarning($"[AddressablesSceneLoadData] Scene '{_sceneName}' is not loaded, nothing to unload");
                return;
            }

            await Addressables.UnloadSceneAsync(_loadHandle.Value).ToUniTask();
            _loadHandle = null;
        }

        #endregion

        #region Editor

#if UNITY_EDITOR
        private void OnUseAddressablesChanged()
        {
            if (_makeAddressable)
            {
                if (string.IsNullOrEmpty(_sceneName)) return;

                string[] guids = AssetDatabase.FindAssets($"t:Scene {_sceneName}");
                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!System.IO.Path.GetFileNameWithoutExtension(path).Equals(_sceneName)) continue;

                    var settings = AddressableAssetSettingsDefaultObject.Settings;
                    if (settings != null && settings.FindAssetEntry(guid) == null)
                        settings.CreateOrMoveEntry(guid, settings.DefaultGroup);

                    _sceneReference = new AssetReference(guid);
                    break;
                }
            }
            else
            {
                if (_sceneReference != null && _sceneReference.RuntimeKeyIsValid())
                {
                    string path = AssetDatabase.GUIDToAssetPath(_sceneReference.AssetGUID);
                    _sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
                    _sceneReference = null;
                }
            }
        }
#endif

        #endregion
    }
}
