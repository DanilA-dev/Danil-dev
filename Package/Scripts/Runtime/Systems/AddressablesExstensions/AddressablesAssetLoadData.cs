using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System.Threading;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
#endif
using UnityEngine.ResourceManagement.AsyncOperations;

namespace D_Dev.AddressablesExstensions
{
    [System.Serializable]
    public class AddressablesAssetLoadData<T> where T : Object
    {
        #region Fields

        [OnValueChanged(nameof(OnUseAddressablesChanged))]
        [SerializeField] protected bool _makeAddressable;
        [HideIf(nameof(_makeAddressable))]
        [OnValueChanged(nameof(OnAssetFieldUpdate))]
        [SerializeField] protected T _asset;
        [ShowIf(nameof(_makeAddressable))]
        [SerializeField] protected AssetReference _assetReference;

        protected AsyncOperationHandle<T>? _loadHandle;

        private UniTask<T>? _pendingLoad;

        #endregion

        #region Properties

        public bool MakeAddressable => _makeAddressable;
        public T Asset => _asset;
        public AssetReference AssetReference => _assetReference;

        #endregion

        #region Public

        public UniTask<T> LoadAsync(CancellationToken cancellationToken = default)
        {
            if (!_makeAddressable)
                return UniTask.FromResult(_asset);

            if (_loadHandle.HasValue && _loadHandle.Value.IsValid())
                return UniTask.FromResult(_loadHandle.Value.Result);

            if (_pendingLoad.HasValue)
                return _pendingLoad.Value.AttachExternalCancellation(cancellationToken);

            _pendingLoad = LoadInternalAsync(cancellationToken);
            return _pendingLoad.Value.AttachExternalCancellation(cancellationToken);
        }

        public virtual void Release()
        {
            _pendingLoad = null;

            if (_loadHandle.HasValue && _loadHandle.Value.IsValid())
            {
                Addressables.Release(_loadHandle.Value);
                _loadHandle = null;
            }
        }

        #endregion

        #region Private

        private async UniTask<T> LoadInternalAsync(CancellationToken cancellationToken)
        {
            if (_assetReference == null)
            {
                Debug.LogError($"AssetReference not set for {typeof(T)}");
                return null;
            }

            _loadHandle = _assetReference.LoadAssetAsync<T>();
            await _loadHandle.Value.ToUniTask(cancellationToken: cancellationToken);

            _pendingLoad = null;
            return _loadHandle.Value.Result;
        }

        private void OnUseAddressablesChanged()
        {
#if UNITY_EDITOR
            if (_makeAddressable)
            {
                if (_asset != null)
                {
                    var settings = AddressableAssetSettingsDefaultObject.Settings;
                    string assetPath = AssetDatabase.GetAssetPath(_asset);
                    string guid = AssetDatabase.AssetPathToGUID(assetPath);
                    if (settings != null && settings.FindAssetEntry(guid) == null)
                    {
                        settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
                        AssetDatabase.SaveAssets();
                    }
                    _assetReference = new AssetReference(guid);
                    _asset = null;
                }
            }
            else
            {
                if (_assetReference != null && _assetReference.RuntimeKeyIsValid())
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(_assetReference.AssetGUID);
                    _asset = (T)AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));
                    _assetReference = null;
                }
            }
#endif
        }

        #endregion

        #region Protected

        protected virtual void OnAssetFieldUpdate() { }

        #endregion
    }
}