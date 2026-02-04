using System.Linq;
using Cysharp.Threading.Tasks;
using D_Dev.Entity;
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

        public async void CreateEntityAsync(int settingsIndex) => await GetEntityAsync(settingsIndex);
        public async void CreateEntityAsync(EntityInfo data) => await GetEntityAsync(data);

        #endregion

        #region Private

        private async UniTask<GameObject> GetEntityAsync(EntityInfo data)
        {
            var spawnSettings = _spawnSettings.FirstOrDefault(s => s.Data.Value == data);
            if (spawnSettings != null)
                for (int i = 0; i < spawnSettings.Amount.Value; i++)
                    return await spawnSettings.Get();
            
            return null;
        }

        private async UniTask<GameObject> GetEntityAsync(int settingsIndex)
        {
            var spawnSettings = _spawnSettings[settingsIndex];
            if (spawnSettings != null)
                for (int i = 0; i < spawnSettings.Amount.Value; i++)
                    return await spawnSettings.Get();
            
            return null;
        }

        #endregion
    }
}
