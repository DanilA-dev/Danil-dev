using System;

namespace D_Dev.SaveSystem
{
    public interface ISaveConfig
    {
        public void Save<T>(string key, T value);
        public T Load<T>(string key, T defaultValue = default);
        public bool HasKey(string key);
        public void LoadRegistry();
        public void DeleteKey(string key);
        public void DeleteAll();
    }
}
