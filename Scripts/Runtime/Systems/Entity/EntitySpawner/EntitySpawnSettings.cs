using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using D_Dev.Entity;
using D_Dev.Extensions;
using D_Dev.PolymorphicValueSystem;
using D_Dev.PositionRotationConfig;
using D_Dev.PositionRotationConfig.RotationSettings;
using D_Dev.RuntimeEntityVariables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace D_Dev.EntitySpawner
{
    [System.Serializable]
    public class EntitySpawnSettings
    {
        #region Fields

        [Title("Data")] 
        [SerializeReference] private PolymorphicValue<EntityInfo> _data;
        [SerializeField] private bool _createOnStart;
        [SerializeReference, Min(1)] private PolymorphicValue<int> _amount;
        [SerializeField] private bool _setActiveOnStart;

        [FoldoutGroup("Position and Rotation")]
        [SerializeField] private bool _setParent;
        [ShowIf(nameof(_setParent))]
        [FoldoutGroup("Position and Rotation")]
        [SerializeField] private Transform _parentTransform;
        [FoldoutGroup("Position and Rotation")]
        [SerializeReference] private BasePositionSettings _positionSettings = new();
        [FoldoutGroup("Position and Rotation")] 
        [SerializeField] private Vector3 _positionOffset;
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
            get => _data.Value;
            set => _data.Value = value;
        }

        public bool CreateOnStart
        {
            get => _createOnStart;
            set => _createOnStart = value;
        }

        public int StartEntitiesAmount
        {
            get => _amount.Value;
            set => _amount.Value = value;
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

        public BasePositionSettings PositionSettings
        {
            get => _positionSettings;
            set => _positionSettings = value;
        }

        public BaseRotationSettings RotationSettings
        {
            get => _rotationSettings;
            set => _rotationSettings = value;
        }

        public bool SetParent
        {
            get => _setParent;
            set => _setParent = value;
        }

        public Transform ParentTransform
        {
            get => _parentTransform;
            set => _parentTransform = value;
        }

        public Vector3 PositionOffset
        {
            get => _positionOffset;
            set => _positionOffset = value;
        }

        public List<PoolableObject> PoolableEntities => _poolableEntities;

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
            if(!_usePool || _pool == null)
                return;

            if(_poolableEntities.Count <= 0)
                return;

            foreach (var poolableEntity in _poolableEntities)
            {
                poolableEntity.OnEntityRelease.RemoveListener(OnPoolableEntityReleased);
                poolableEntity.OnEntityDestroy.RemoveListener(OnPoolableEntityDestroyed);
                _pool.Release(poolableEntity);
            }
            
            _pool.Dispose();
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
            for (int i = 0; i < _amount.Value; i++)
                await CreateEntity();
        }
        
        private void CreatePool()
        {
            int index = 0;

            _pool = new ObjectPool<PoolableObject>(
                createFunc: () => _poolableEntities[index++],
                actionOnGet: p => { if (p != null && p.gameObject != null) p.gameObject.SetActive(true); },
                actionOnRelease: p => { if (p != null && p.gameObject != null) p.gameObject.SetActive(false); },
                actionOnDestroy: p => { if (p != null && p.gameObject != null) Object.Destroy(p.gameObject); },
                collectionCheck: false,
                defaultCapacity: _poolableEntities.Count,
                maxSize: _poolableEntities.Count
            );
        }
        
        private async UniTask PrewarmPool()
        {
            _poolableEntities = new List<PoolableObject>();
            for (int i = 0; i < _amount.Value; i++)
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

            obj.transform.position = _positionSettings.GetPosition() + _positionOffset;
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
            _pool.Release(poolableObject);
            _poolableEntities.TryRemove(poolableObject);
        }

        private void OnPoolableEntityReleased(PoolableObject poolableObject)
        {
            ReleasePoolObject(poolableObject);
        }

        #endregion
    }
}
