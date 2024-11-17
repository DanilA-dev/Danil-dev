using Sirenix.OdinInspector;
using Sirenix.Serialization;
using SpawnSettings;
using UnityEngine;
using UnityEngine.Pool;

namespace Entities.EntitiesInfo
{
    [System.Serializable]
    public class EntitySpawnSettings
    {
        [field: SerializeField] public EntityInfo Data { get; private set; }
        [field: SerializeField] public bool UsePool { get; private set; }
        [ShowIf(nameof(UsePool))]
        [SerializeField] private bool _poolCollectionCheck;
        [ShowIf(nameof(UsePool))]
        [SerializeField] private int _poolDefaultCapacity;
        [ShowIf(nameof(UsePool))]
        [SerializeField] private int _poolMaxSize;
        [field: OdinSerialize] public ISpawnSettings SpawnSettings { get; private set; }

        private ObjectPool<GameObject> _pool;

        public void Init()
        {
            if (UsePool)
                _pool = new ObjectPool<GameObject>(
                    createFunc: CreateObject,
                    actionOnGet: _ => _.SetActive(true),
                    actionOnRelease: _ => _.SetActive(false),
                    actionOnDestroy: _ => GameObject.Destroy(_.gameObject),
                    collectionCheck: _poolCollectionCheck,
                    defaultCapacity: _poolDefaultCapacity,
                    maxSize: _poolMaxSize);
        }

        public GameObject Get() => UsePool ? _pool?.Get() : CreateObject();

        public void Release(GameObject gameObject)
        {
            if(UsePool)
                _pool?.Release(gameObject);
            else
                GameObject.Destroy(gameObject);
        }
            
        private GameObject CreateObject() => SpawnSettings.Spawn(Data.EntityPrefab.GameObject);
    }
}