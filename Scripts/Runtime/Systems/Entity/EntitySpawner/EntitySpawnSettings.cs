using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using D_Dev.Base;
using D_Dev.EntityVariable;
using D_Dev.Extensions;
using D_Dev.PositionRotationConfig;
using D_Dev.PositionRotationConfig.RotationSettings;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEngine.Pool;

namespace D_Dev.EntitySpawner
{
    [System.Serializable]
    public class EntitySpawnSettings
    {
        #region Fields

        [Title("Data")] 
        [SerializeField] private EntityInfo _data;
        [SerializeField] private bool _createOnStart;
        [ShowIf(nameof(_createOnStart))]
        [SerializeField, Min(1)] private int _startEntitiesAmount;
        [SerializeField] private bool _setActiveOnStart;

        [FoldoutGroup("Position and Rotation")]
        [SerializeField] private bool _setParent;
        [ShowIf(nameof(_setParent))]
        [FoldoutGroup("Position and Rotation")]
        [SerializeField] private Transform _parentTransform;
        [FoldoutGroup("Position and Rotation")]
        [SerializeReference] private BasePositionSettings _positionSettings = new();
        [FoldoutGroup("Position and Rotation")]
        [SerializeReference] private BaseRotationSettings _rotationSettings = new();
        [FoldoutGroup("Pool")]
        [SerializeField] private bool _usePool;
        [FoldoutGroup("Pool")]
        [ShowIf(nameof(_usePool))]
        [SerializeField] private bool _applyPosConfigOnGet;
        [FoldoutGroup("Pool")]
        [ShowIf(nameof(_usePool))]
        [SerializeField] private bool _poolCollectionCheck;
        [FoldoutGroup("Pool")]
        [ShowIf(nameof(_usePool))]
        [SerializeField] private int _poolDefaultCapacity;
        [FoldoutGroup("Pool")]
        [ShowIf(nameof(_usePool))]
        [SerializeField] private int _poolMaxSize;

        private ObjectPool<PoolableObject> _pool;
        private List<PoolableObject> _poolableEntities;

        #endregion

        #region Properties

        public EntityInfo Data
        {
            get => _data;
            set => _data = value;
        }

        public bool CreateOnStart
        {
            get => _createOnStart;
            set => _createOnStart = value;
        }

        public int StartEntitiesAmount
        {
            get => _startEntitiesAmount;
            set => _startEntitiesAmount = value;
        }

        public bool SetActiveOnStart
        {
            get => _setActiveOnStart;
            set => _setActiveOnStart = value;
        }
        public bool UsePool
        {
            get => _usePool;
            set => _usePool = value;
        }

        public bool ApplyPosConfigOnGet
        {
            get => _applyPosConfigOnGet;
            set => _applyPosConfigOnGet = value;
        }

        public bool PoolCollectionCheck
        {
            get => _poolCollectionCheck;
            set => _poolCollectionCheck = value;
        }

        public int PoolDefaultCapacity
        {
            get => _poolDefaultCapacity;
            set => _poolDefaultCapacity = value;
        }

        public int PoolMaxSize
        {
            get => _poolMaxSize;
            set => _poolMaxSize = value;
        }

        #endregion

        #region Public

        public async UniTask Init()
        {
            if (_usePool)
            {
                await PrewarmPool();
                CreatePool();
            }

            if (_createOnStart)
                await PreCreateEntities();
        }

        public void DisposePool()
        {
            if(!_usePool)
                return;
            
            if(_poolableEntities.Count <= 0)
                return;

            foreach (var poolableEntity in _poolableEntities)
            {
                poolableEntity.OnEntityRelease.RemoveListener(OnPoolableEntityReleased);
                poolableEntity.OnEntityDestroy.RemoveListener(OnPoolableEntityDestroyed);
            }
            _poolableEntities.Clear();
        }

        public async UniTask<GameObject> Get()
        {
            GameObject returnObj = null;
            if (_usePool)
            {
                returnObj = _pool?.Get().gameObject;
                if (returnObj != null && _applyPosConfigOnGet)
                {
                    returnObj.transform.position = _positionSettings.GetPosition();
                    returnObj.transform.rotation = _rotationSettings.GetRotation();
                }
            }
            else
                returnObj = await CreateEntity();
            
            return returnObj;
        }

        public void ReleasePoolObject(PoolableObject poolableObject)
        {
            if(_usePool)
                _pool?.Release(poolableObject);
        }

        #endregion

        #region Private

        private async UniTask PreCreateEntities()
        {
            for (int i = 0; i < _startEntitiesAmount; i++)
                await CreateEntity();
        }
        
        private void CreatePool()
        {
            int index = 0;

            _pool = new ObjectPool<PoolableObject>(
                createFunc: () => _poolableEntities[index++],
                actionOnGet: p => p.gameObject.SetActive(true),
                actionOnRelease: p => p.gameObject.SetActive(false),
                actionOnDestroy: p => Object.Destroy(p.gameObject),
                collectionCheck: false,
                defaultCapacity: _poolableEntities.Count,
                maxSize: _poolableEntities.Count
            );
        }
        
        private async UniTask PrewarmPool()
        {
            for (int i = 0; i < _startEntitiesAmount; i++)
            {
                var go = await CreateEntity();
                var poolable = go.GetComponent<PoolableObject>();
                go.SetActive(false);
                _poolableEntities.Add(poolable);
            }
        }
        
        private async UniTask<GameObject> CreateEntity()
        {
            
            GameObject obj = null;
            var entityAsset = Data.EntityPrefab;
            obj = await entityAsset.InstantiateAsync();
            var entityTransform = obj.transform;
            if(_setParent && _parentTransform != null)
                entityTransform.SetParent(_parentTransform);
            
            obj.transform.position = _positionSettings.GetPosition();
            obj.transform.rotation = _rotationSettings.GetRotation();
            
            obj.SetActive(_setActiveOnStart);
            
            if(obj.TryGetComponent(out RuntimeEntityVariablesContainer runtimeEntityVariablesContainer))
                runtimeEntityVariablesContainer.Init(Data.Variables);
            
            if (_usePool && obj.TryGetComponent(out PoolableObject poolableEntity))
            {
                poolableEntity.OnEntityRelease.AddListener(OnPoolableEntityReleased);
                poolableEntity.OnEntityDestroy.AddListener(OnPoolableEntityDestroyed);
                _poolableEntities.Add(poolableEntity);
            }
            return obj;
        }

        private void OnPoolableEntityDestroyed(PoolableObject poolableObject)
        {
            poolableObject.OnEntityRelease.RemoveListener(OnPoolableEntityReleased);
            poolableObject.OnEntityDestroy.RemoveListener(OnPoolableEntityDestroyed);
            _poolableEntities.TryRemove(poolableObject);
        }

        private void OnPoolableEntityReleased(PoolableObject poolableObject)
        {
            ReleasePoolObject(poolableObject);
        }

        #endregion
    }
}
