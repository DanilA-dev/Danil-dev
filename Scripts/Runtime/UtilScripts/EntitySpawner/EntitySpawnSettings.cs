using System.Collections.Generic;
using D_Dev.Base;
using D_Dev.Extensions;
using D_Dev.PositionRotationConfig;
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
        [SerializeField] private EntityInfo _data;
        [SerializeField] private bool _createOnStart;
        [ShowIf(nameof(_createOnStart))]
        [SerializeField, Min(1)] private int _startEntitiesAmount;
        [SerializeField] private bool _setActiveOnStart;
        [FoldoutGroup("Position and Rotation")]
        [HideLabel]
        [SerializeField] private PositionConfig _posConfig;
        [FoldoutGroup("Position and Rotation")]
        [HideLabel]
        [SerializeField] private RotationConfig _rotConfig;
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

        public PositionConfig PosConfig
        {
            get => _posConfig;
            set => _posConfig = value;
        }
        
        public RotationConfig RotConfig
        {
            get => _rotConfig;
            set => _rotConfig = value;
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

        public void Init()
        {
            if (_usePool)
            {
                _poolableEntities = new();
                _pool = new ObjectPool<PoolableObject>(
                    createFunc:() => CreateObject().GetComponent<PoolableObject>(),
                    actionOnGet: _ => _.gameObject.SetActive(true),
                    actionOnRelease: _ => _.gameObject.SetActive(false),
                    actionOnDestroy: _ => GameObject.Destroy(_.gameObject),
                    collectionCheck: _poolCollectionCheck,
                    defaultCapacity: _poolDefaultCapacity,
                    maxSize: _poolMaxSize);
            }

            if (_createOnStart)
                PreCreateObjects();
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

        public GameObject Get()
        {
            GameObject returnObj = null;
            if (_usePool)
            {
                returnObj = _pool?.Get().gameObject;
                if (returnObj != null && _applyPosConfigOnGet)
                {
                    returnObj.transform.position = _posConfig.GetPosition();
                    returnObj.transform.rotation = _rotConfig.GetRotation();
                }

            }
            else
                returnObj = CreateObject();
            
            return returnObj;
        }

        public void ReleasePoolObject(PoolableObject poolableObject)
        {
            if(_usePool)
                _pool?.Release(poolableObject);
        }

        #endregion

        #region Private

        private void PreCreateObjects()
        {
            for (int i = 0; i < _startEntitiesAmount; i++)
            {
                if (!_usePool)
                    CreateObject();
                else
                {
                    var pooledObj = _pool?.Get();
                    _poolableEntities.Add(pooledObj);
                }
            }
            
            if(_poolableEntities != null && _poolableEntities.Count > 0)
                foreach (var poolableEntity in _poolableEntities)
                    poolableEntity.Release();
        }
        
        private GameObject CreateObject()
        {
            GameObject obj = null;
            var entity = Data.EntityPrefab;
            obj = GameObject.Instantiate(entity);
            var entityTransform = obj.transform;
            _posConfig.SetPosition(ref entityTransform);
            _rotConfig.SetRotation(ref entityTransform);
            obj.SetActive(_setActiveOnStart);
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
