using System.Linq;
using D_Dev.Base;
using UnityEngine;

namespace D_Dev.EntitySpawner
{
    public class EntitiesSpawner : MonoBehaviour
    {
        #region Fields

        [SerializeField] private EntitySpawnSettings[] _spawnSettings;

        #endregion

        #region Monobehaviour

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
                entitySpawnSettings.DisposePool();
        }

        #endregion

        #region Public

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

        #endregion
    }
}
