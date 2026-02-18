using System;
using System.Collections.Generic;
using System.IO;
using D_Dev.SaveSystem.Converters;
using Newtonsoft.Json;
using UnityEngine;

namespace D_Dev.SaveSystem
{
    public class JsonFileSaveConfig : ISaveConfig
    {
        #region Fields

        private JsonSerializerSettings _settings;
        private HashSet<string> _keys;

        #endregion
        
        #region Private

        private string GetFilePath(string key) =>
            Path.Combine(Application.persistentDataPath, $"{key}.json");

        private string RegistryPath =>
            Path.Combine(Application.persistentDataPath, "_SaveKeys.json");

        private void SaveRegistry()
        {
            File.WriteAllText(RegistryPath, JsonConvert.SerializeObject(_keys, Formatting.Indented));
        }

        #endregion

        #region Constructors

        public JsonFileSaveConfig()
        {
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = new List<JsonConverter>
                {
                    new Vector2Converter(),
                    new Vector3Converter(),
                    new QuaternionConverter(),
                    new ColorConverter()
                }
            };
        }

        #endregion
        
        #region Public

        public void LoadRegistry()
        {
            if (File.Exists(RegistryPath))
            {
                try
                {
                    _keys = JsonConvert.DeserializeObject<HashSet<string>>(File.ReadAllText(RegistryPath));
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
                string path = GetFilePath(key);
                string json = JsonConvert.SerializeObject(value, _settings);
                File.WriteAllText(path, json);

                if (_keys.Add(key))
                    SaveRegistry();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonFileSaveConfig] Save error: {ex.Message}");
            }
        }

        public T Load<T>(string key, T defaultValue = default)
        {
            try
            {
                string path = GetFilePath(key);
                if (!File.Exists(path))
                    return defaultValue;

                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json, _settings);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonFileSaveConfig] Load error: {ex.Message}");
                return defaultValue;
            }
        }

        public bool HasKey(string key) => File.Exists(GetFilePath(key));

        public void DeleteKey(string key)
        {
            string path = GetFilePath(key);
            if (File.Exists(path))
            {
                File.Delete(path);
                if (_keys.Remove(key))
                    SaveRegistry();
            }
        }

        public void DeleteAll()
        {
            foreach (var key in _keys)
            {
                string path = GetFilePath(key);
                if (File.Exists(path))
                    File.Delete(path);
            }

            _keys.Clear();
            SaveRegistry();
        }

        #endregion
    }
}
