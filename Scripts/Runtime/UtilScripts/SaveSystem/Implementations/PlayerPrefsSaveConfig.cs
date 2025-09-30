using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace D_Dev.SaveSystem
{
    public class PlayerPrefsSaveConfig : ISaveConfig
    {
        #region Fields

        private const string KeyRegistry = "_SaveKeys"; 
        private HashSet<string> _keys;

        #endregion
        
        #region Private
        private void SaveRegistry()
        {
            PlayerPrefs.SetString(KeyRegistry, JsonConvert.SerializeObject(_keys));
            PlayerPrefs.Save();
        }

        #endregion

        #region Public
        
        public void LoadRegistry()
        {
            if (PlayerPrefs.HasKey(KeyRegistry))
            {
                try
                {
                    _keys = JsonConvert.DeserializeObject<HashSet<string>>(PlayerPrefs.GetString(KeyRegistry));
                }
                catch
                {
                    _keys = new HashSet<string>();
                }
            }
            else
                _keys = new HashSet<string>();
        }

        public void Save<T>(string key, T value)
        {
            try
            {
                string json = JsonConvert.SerializeObject(value, Formatting.None);
                PlayerPrefs.SetString(key, json);

                if (_keys.Add(key))
                    SaveRegistry();

                PlayerPrefs.Save();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[PlayerPrefsSaveConfig] Save error: {ex.Message}");
            }
        }

        public T Load<T>(string key, T defaultValue = default)
        {
            try
            {
                if (!PlayerPrefs.HasKey(key))
                    return defaultValue;

                string json = PlayerPrefs.GetString(key);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[PlayerPrefsSaveConfig] Load error: {ex.Message}");
                return defaultValue;
            }
        }

        public bool HasKey(string key) => PlayerPrefs.HasKey(key);

        public void DeleteKey(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                if (_keys.Remove(key))
                    SaveRegistry();
            }
        }

        public void DeleteAll()
        {
            foreach (var key in _keys)
                PlayerPrefs.DeleteKey(key);

            _keys.Clear();
            SaveRegistry();
        }

        #endregion
    }
}
