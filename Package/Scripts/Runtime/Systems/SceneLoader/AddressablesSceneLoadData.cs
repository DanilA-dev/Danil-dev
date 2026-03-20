using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace D_Dev.AddressablesExstensions
{
    [System.Serializable]
    public class AddressablesSceneLoadData
    {
        #region Fields

        [SerializeField] private bool _makeAddressable;

        [ShowIf(nameof(_makeAddressable))]
        [SerializeField] private AssetReference _sceneReference;

        private AsyncOperationHandle<SceneInstance>? _loadHandle;

        #endregion

        #region Properties

        public bool IsAddressable => _makeAddressable;
        public bool IsLoaded => _loadHandle.HasValue && _loadHandle.Value.IsValid()
                                && _loadHandle.Value.Result.Scene.isLoaded;

        #endregion

        #region Public

        public async UniTask LoadAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            if (!_makeAddressable)
            {
                await SceneManager.LoadSceneAsync(sceneName, mode);
                return;
            }

            if (_sceneReference == null || !_sceneReference.RuntimeKeyIsValid())
            {
                Debug.LogError($"[AddressablesSceneLoadData] SceneReference is not set or invalid for '{sceneName}'");
                return;
            }

            if (_loadHandle.HasValue && _loadHandle.Value.IsValid())
            {
                Debug.LogWarning($"[AddressablesSceneLoadData] Scene '{sceneName}' is already loaded");
                return;
            }

            _loadHandle = Addressables.LoadSceneAsync(_sceneReference, mode);
            await _loadHandle.Value.ToUniTask();
        }

        public async UniTask UnloadAsync(string sceneName)
        {
            if (!_makeAddressable)
            {
                await SceneManager.UnloadSceneAsync(sceneName);
                return;
            }

            if (!_loadHandle.HasValue || !_loadHandle.Value.IsValid())
            {
                Debug.LogWarning($"[AddressablesSceneLoadData] Scene '{sceneName}' is not loaded, nothing to unload");
                return;
            }

            await Addressables.UnloadSceneAsync(_loadHandle.Value).ToUniTask();
            _loadHandle = null;
        }

        #endregion
    }
}