using System.Collections.Generic;
using Extensions;
using PositionSetter;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Entities.EntitiesInfo
{
    [System.Serializable]
    public class EntitySpawnSettings
    {
        [FoldoutGroup("Data")] 
        [SerializeField] private EntityInfo _data;
        [FoldoutGroup("Data")] 
        [SerializeField, Min(1)] private int _entitiesAmount;
        [FoldoutGroup("Data")] 
        [SerializeField] private bool _createOnStart;
        [FoldoutGroup("Data")] 
        [SerializeField] private bool _setActiveOnStart;
        [FoldoutGroup("Position and Rotation")]
        [HideLabel]
        [SerializeField] private PositionConfig _posConfig;
        [FoldoutGroup("Pool")]
        [SerializeField] private bool _usePool;
        [FoldoutGroup("Pool")]
        [ShowIf(nameof(_usePool))]
        [SerializeField] private bool _poolCollectionCheck;
        [FoldoutGroup("Pool")]
        [ShowIf(nameof(_usePool))]
        [SerializeField] private int _poolDefaultCapacity;
        [FoldoutGroup("Pool")]
        [ShowIf(nameof(_usePool))]
        [SerializeField] private int _poolMaxSize;

        private ObjectPool<GameObject> _pool;
        private List<PoolableEntity> _poolableEntities;

        public EntityInfo Data => _data;

        
        public void Init()
        {
            _poolableEntities = new();
            
            if (_usePool)
                _pool = new ObjectPool<GameObject>(
                    createFunc: CreateObject,
                    actionOnGet: _ => _.SetActive(true),
                    actionOnRelease: _ => _.SetActive(false),
                    actionOnDestroy: _ => GameObject.Destroy(_.gameObject),
                    collectionCheck: _poolCollectionCheck,
                    defaultCapacity: _poolDefaultCapacity,
                    maxSize: _poolMaxSize);

            if (_createOnStart)
                Get();
        }

        public void Dispose()
        {
            if(_poolableEntities.Count <= 0)
                return;

            foreach (var poolableEntity in _poolableEntities)
            {
                poolableEntity.OnEntityRelease -= OnPoolableEntityReleased;
                poolableEntity.OnEntityDestroy -= OnPoolableEntityDestroyed;
            }
            _poolableEntities.Clear();
        }

        public GameObject Get() => _usePool ? _pool?.Get() : CreateObject();

        public void Release(GameObject gameObject)
        {
            if(_usePool)
                _pool?.Release(gameObject);
            else
                GameObject.Destroy(gameObject);
        }
            
        private GameObject CreateObject()
        {
            GameObject obj = null;
            for (int i = 0; i < _entitiesAmount; i++)
            {
                var entity = Data.EntityPrefab;
                if (entity.TryGetComponent(out PoolableEntity poolableEntity))
                {
                    poolableEntity.OnEntityRelease += OnPoolableEntityReleased;
                    poolableEntity.OnEntityDestroy += OnPoolableEntityDestroyed;
                    _poolableEntities.Add(poolableEntity);
                }
                obj = GameObject.Instantiate(entity, _posConfig.GetPosition(), _posConfig.GetRotation());
                obj.SetActive(_setActiveOnStart);
            }
            return obj;
        }

        private void OnPoolableEntityDestroyed(PoolableEntity poolableEntity)
        {
            poolableEntity.OnEntityRelease -= OnPoolableEntityReleased;
            poolableEntity.OnEntityDestroy -= OnPoolableEntityDestroyed;
            _poolableEntities.TryRemove(poolableEntity);
        }

        private void OnPoolableEntityReleased(PoolableEntity poolableEntity)
        {
            Release(poolableEntity.gameObject);
        }
    }
}