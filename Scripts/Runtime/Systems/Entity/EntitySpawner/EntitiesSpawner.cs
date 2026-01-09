using System.Linq;
using Cysharp.Threading.Tasks;
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

        private async void Start()
        {
            if(_spawnSettings.Length <= 0)
                return;

            foreach (var entitySpawnSettings in _spawnSettings)
                await entitySpawnSettings.Init();
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

        public async UniTask CreateEntityAsync(int settingsIndex) => await GetEntityAsync(settingsIndex);
        public async UniTask CreateEntityAsync(EntityInfo data) => await GetEntityAsync(data);

        #endregion

        #region Private

        private async UniTask<GameObject> GetEntityAsync(EntityInfo data)
        {
            var spawnSettings = _spawnSettings.FirstOrDefault(s => s.Data == data);
            return spawnSettings != null ? await spawnSettings.Get() : null;
        }

        private async UniTask<GameObject> GetEntityAsync(int settingsIndex)
        {
            var spawnSettings = _spawnSettings[settingsIndex];
            return spawnSettings != null ? await spawnSettings.Get() : null;
        }

        #endregion
    }
}
