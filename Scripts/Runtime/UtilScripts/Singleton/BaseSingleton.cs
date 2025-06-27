using UnityEngine;

namespace D_dev.UtilScripts.Singleton
{
    public class BaseSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Fields
        
        private static T _instance;
        private static readonly object _lock = new object();
        
        #endregion

        #region Properties
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();

                        if (_instance == null)
                        {
                            var singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = $"{typeof(T).Name} (Singleton)";
                            DontDestroyOnLoad(singletonObject);
                            
                            Debug.Log($"[Singleton] Created new instance of {typeof(T)}");
                        }
                        else
                        {
                            Debug.Log($"[Singleton] Using existing instance of {typeof(T)}");
                        }
                    }
                    return _instance;
                }
            }
        }
        #endregion

        #region Monobehaviour
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Debug.LogWarning($"[Singleton] Another instance of {typeof(T)} already exists. Destroying this one.");
                Destroy(gameObject);
            }
        }
       
        #endregion
    }
}