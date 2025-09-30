using D_Dev.Singleton;
using UnityEngine;

namespace D_Dev.SaveSystem
{
    public class GlobalSaveService : BaseSingleton<GlobalSaveService>
    {
        #region Fields

        [SerializeReference] private ISaveConfig _saveConfig;

        #endregion

        #region Monobehaviour

        protected override void Awake()
        {
            base.Awake();
            
            if (_saveConfig == null)
            {
                _saveConfig = new PlayerPrefsSaveConfig();
                Debug.Log("[SaveService] Using default PlayerPrefsSaveConfig");
            }
            _saveConfig?.LoadRegistry();
        }

        #endregion

        #region Public

        public void Save<T>(string key, T value) => _saveConfig.Save(key, value);
        public T Load<T>(string key, T defaultValue = default) => _saveConfig.Load(key, defaultValue);
        public bool HasKey(string key) => _saveConfig.HasKey(key);
        public void DeleteKey(string key) => _saveConfig.DeleteKey(key);
        public void DeleteAll() => _saveConfig.DeleteAll();

        #endregion
    }
}
