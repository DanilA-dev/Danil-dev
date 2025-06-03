using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace D_dev.Scripts.EventHandler
{
    public static class CustomEventHandler
    {
        #region Fields

        private static Dictionary<CustomEventType, Delegate> _events = new();

        #endregion

        #region Private

        private static void Add(CustomEventType eventType, Delegate value)
        {
            if (!_events.TryGetValue(eventType, out var del))
            {
                _events[eventType] = value;
            }
            else
            {
                try
                {
                    _events[eventType] = Delegate.Combine(del, value);
                }
                catch (Exception e) when(e is ArgumentException)
                {
                   Debug.LogError($"[CustomEventHandler] : Add event fail - {e}");
                }
            }
        }

        private static void Remove(CustomEventType eventType, Delegate value)
        {
            if (!_events.TryGetValue(eventType, out var del))
                return;

            try
            {
                var e = Delegate.Remove(value, value);
                if(e != null)
                    _events[eventType] = e;
                else
                    _events.Remove(eventType);
            }
            catch (Exception e) when (e is ArgumentException)
            {
                Debug.LogError($"[CustomEventHandler] : Remove event fail - {e}");
            }
           
        }

        #endregion
        
        #region Public

        #region Adders

        public static void AddListener(CustomEventType eventType, Action action)
        {
           Add(eventType, action);
        }

        public static void AddListener<T1>(CustomEventType eventType, Action<T1> action)
        {
            Add(eventType, action);
        }

        #endregion
        
        #region Removers
        
        public static void RemoveListener(CustomEventType eventType, Action action)
        {
           Remove(eventType, action);
        }

        public static void RemoveListener<T1>(CustomEventType eventType, Action<T1> action)
        {
            Remove(eventType, action);
        }
        
        #endregion

        #region Invokers

        public static void Invoke(CustomEventType eventType)
        {
            if (_events.TryGetValue(eventType, out var del))
               (del as Action)?.Invoke();
        }
        
        public static void Invoke<T1>(CustomEventType eventType, T1 arg1)
        {
            if (_events.TryGetValue(eventType, out var del))
                (del as Action<T1>)?.Invoke(arg1);
        }
        #endregion

        #endregion

#if UNITY_EDITOR
        
        #region Domain Reload

        [InitializeOnEnterPlayMode]
        public static void ResetDomain()
        {
            _events.Clear();
        }

        #endregion
#endif
    }
}