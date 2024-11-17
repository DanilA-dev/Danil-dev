using System.Linq;
using UnityEngine;

namespace Entities.EntitiesInfo
{
    public class EntitiesSpawner : MonoBehaviour
    {
        [SerializeField] private EntitySpawnSettings[] _spawnSettings;

        private void Start()
        {
            if(_spawnSettings.Length <= 0)
                return;
            
            foreach (var entitySpawnSettings in _spawnSettings)
                entitySpawnSettings.Init();
        }

        private void OnDisable()
        {
            if(_spawnSettings.Length <= 0)
                return;
            
            foreach (var entitySpawnSettings in _spawnSettings)
                entitySpawnSettings.Dispose();
        }

        public GameObject CreateEntity(EntityInfo data)
        {
            var spawnSettings = _spawnSettings.FirstOrDefault(s => s.Data == data);
            return spawnSettings?.Get();
        }
        
        public GameObject CreateEntity(int settingsIndex)
        {
            var spawnSettings = _spawnSettings[settingsIndex];
            return spawnSettings?.Get();
        }
    }
}