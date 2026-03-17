using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading;

namespace D_Dev.AddressablesExstensions
{
    [System.Serializable]
    public class AddressablesGameObjectLoadData : AddressablesAssetLoadData<GameObject>
    {
        #region Fields

        private AsyncOperationHandle<GameObject>? _instantiateHandle;
        private GameObject _instance;

        #endregion

        #region Public

        public async UniTask<GameObject> InstantiateAsync(
            Transform parent = null,
            bool worldPositionStays = true,
            CancellationToken cancellationToken = default)
        {
            if (_instance != null)
                return _instance;

            if (!MakeAddressable)
                return Instantiate(parent, worldPositionStays);

            if (AssetReference == null)
            {
                Debug.LogError("AssetReference not set for GameObject");
                return null;
            }

            if (_loadHandle.HasValue && _loadHandle.Value.IsValid())
            {
                _instance = Object.Instantiate(_loadHandle.Value.Result, parent, worldPositionStays);
                return _instance;
            }

            _instantiateHandle = AssetReference.InstantiateAsync(parent, worldPositionStays);
            await _instantiateHandle.Value.ToUniTask(cancellationToken: cancellationToken);

            _instance = _instantiateHandle.Value.Result;
            return _instance;
        }

        public GameObject Instantiate(Transform parent = null, bool worldPositionStays = true)
        {
            if (_asset == null)
                return null;

            _instance = Object.Instantiate(_asset, parent, worldPositionStays);
            return _instance;
        }

        #endregion

        #region Overrides

        public override void Release()
        {
            if (_instantiateHandle.HasValue && _instantiateHandle.Value.IsValid())
            {
                Addressables.ReleaseInstance(_instantiateHandle.Value.Result);
                _instantiateHandle = null;
            }
            else if (_instance != null)
            {
                Object.Destroy(_instance);
            }

            _instance = null;
            base.Release();
        }

        #endregion
    }
}