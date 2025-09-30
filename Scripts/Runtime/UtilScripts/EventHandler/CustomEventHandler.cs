using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace D_Dev.EventHandler
{
    public static class CustomEventHandler
    {
        #region Fields

        private static Dictionary<CustomEventType, Delegate> _events = new();

        #endregion

        #region Private

        private static void Add(CustomEventType eventType, Delegate value,
            Action<Delegate> onAddCallBack = null,
            Action<Delegate> onFailCallback = null)
        {
            if (!_events.TryGetValue(eventType, out var del))
            {
                _events[eventType] = value;
                onAddCallBack?.Invoke(value);
            }
            else
            {
                try
                {
                    _events[eventType] = Delegate.Combine(del, value);
                    onAddCallBack?.Invoke(value);
                }
                catch (Exception e) when(e is ArgumentException)
                {
                    if (onFailCallback == null)
                        Debug.LogError($"[CustomEventHandler] : Add event fail - {e}");
                    else
                        onFailCallback?.Invoke(value);
                }
            }
        }

        private static void Remove(CustomEventType eventType, Delegate value,
            Action<Delegate> onRemoveCallback = null,
            Action<Delegate> onFailCallback = null)
        {
            if (!_events.TryGetValue(eventType, out var del))
                return;

            try
            {
                var e = Delegate.Remove(del, value);
                if (e != null)
                {
                    _events[eventType] = e;
                    onRemoveCallback?.Invoke(e);
                }
                else
                {
                    _events.Remove(eventType);
                    onRemoveCallback?.Invoke(value);
                }
                    
            }
            catch (Exception e) when (e is ArgumentException)
            {
                if (onFailCallback == null)
                    Debug.LogError($"[CustomEventHandler] : Remove event fail - {e}");
                else
                    onFailCallback?.Invoke(value);
            }
           
        }

        #endregion
        
        #region Public

        #region Adders

        public static void AddListener(CustomEventType eventType, Action action,
            Action<Delegate> onAddCallBack = null,
            Action<Delegate> onFailCallback = null) => Add(eventType, action, onAddCallBack, onFailCallback);
        public static void AddListener<T1>(CustomEventType eventType, Action<T1> action,
            Action<Delegate> onAddCallBack = null,
            Action<Delegate> onFailCallback = null) => Add(eventType, action, onAddCallBack, onFailCallback);
        public static void AddListener<T1, T2>(CustomEventType eventType, Action<T1, T2> action,
            Action<Delegate> onAddCallBack = null,
            Action<Delegate> onFailCallback = null) => Add(eventType, action, onAddCallBack, onFailCallback);
        public static void AddListener<T1, T2, T3>(CustomEventType eventType, Action<T1, T2, T3> action,
            Action<Delegate> onAddCallBack = null,
            Action<Delegate> onFailCallback = null) => Add(eventType, action, onAddCallBack, onFailCallback);
        public static void AddListener<T1, T2, T3, T4>(CustomEventType eventType, Action<T1, T2, T3, T4> action,
            Action<Delegate> onAddCallBack = null,
            Action<Delegate> onFailCallback = null) => Add(eventType, action, onAddCallBack, onFailCallback);
        public static void AddListener<T1, T2, T3, T4, T5>(CustomEventType eventType, Action<T1, T2, T3, T4, T5> action,
            Action<Delegate> onAddCallBack = null,
            Action<Delegate> onFailCallback = null) => Add(eventType, action, onAddCallBack, onFailCallback);
        public static void AddListener<T1, T2, T3, T4, T5, T6>(CustomEventType eventType, Action<T1, T2, T3, T4, T5, T6> action,
            Action<Delegate> onAddCallBack = null,
            Action<Delegate> onFailCallback = null) => Add(eventType, action, onAddCallBack, onFailCallback);
        
        
        #endregion
        
        #region Removers
        
        public static void RemoveListener(CustomEventType eventType, Action action,
            Action<Delegate> onRemoveCallback = null,
            Action<Delegate> onFailCallback = null) => Remove(eventType, action, onRemoveCallback, onFailCallback);
        public static void RemoveListener<T1>(CustomEventType eventType, Action<T1> action,
            Action<Delegate> onRemoveCallback = null,
            Action<Delegate> onFailCallback = null) => Remove(eventType, action, onRemoveCallback, onFailCallback);
        public static void RemoveListener<T1, T2>(CustomEventType eventType, Action<T1, T2> action,
            Action<Delegate> onRemoveCallback = null,
            Action<Delegate> onFailCallback = null) => Remove(eventType, action, onRemoveCallback, onFailCallback);
        public static void RemoveListener<T1, T2, T3>(CustomEventType eventType, Action<T1, T2, T3> action,
            Action<Delegate> onRemoveCallback = null,
            Action<Delegate> onFailCallback = null) => Remove(eventType, action, onRemoveCallback, onFailCallback);
        public static void RemoveListener<T1, T2, T3, T4>(CustomEventType eventType, Action<T1, T2, T3, T4> action,
            Action<Delegate> onRemoveCallback = null,
            Action<Delegate> onFailCallback = null) => Remove(eventType, action, onRemoveCallback, onFailCallback);
        public static void RemoveListener<T1, T2, T3, T4, T5>(CustomEventType eventType, Action<T1, T2, T3, T4, T5> action,
            Action<Delegate> onRemoveCallback = null,
            Action<Delegate> onFailCallback = null) => Remove(eventType, action, onRemoveCallback, onFailCallback);
        public static void RemoveListener<T1, T2, T3, T4, T5, T6>(CustomEventType eventType, Action<T1, T2, T3, T4, T5, T6> action,
            Action<Delegate> onRemoveCallback = null,
            Action<Delegate> onFailCallback = null) => Remove(eventType, action, onRemoveCallback, onFailCallback);
        
        #endregion

        #region Invokers

        public static void Invoke(CustomEventType eventType, Action onInvokeCallback = null)
        {
            if (_events.TryGetValue(eventType, out var del))
            {
               (del as Action)?.Invoke();
               onInvokeCallback?.Invoke();
            }
        }
        
        public static void Invoke<T1>(CustomEventType eventType, T1 arg1, Action onInvokeCallback = null)
        {
            if (_events.TryGetValue(eventType, out var del))
            {
                (del as Action<T1>)?.Invoke(arg1);
                onInvokeCallback?.Invoke();
            }
        }
        
        public static void Invoke<T1, T2>(CustomEventType eventType, T1 arg1, T2 arg2, Action onInvokeCallback = null)
        {
            if (_events.TryGetValue(eventType, out var del))
            {
                (del as Action<T1,T2>)?.Invoke(arg1,arg2);
                onInvokeCallback?.Invoke();
            }
        }
        
        public static void Invoke<T1, T2, T3>(CustomEventType eventType, T1 arg1, T2 arg2, T3 arg3,
            Action onInvokeCallback = null)
        {
            if (_events.TryGetValue(eventType, out var del))
            {
                (del as Action<T1, T2, T3>)?.Invoke(arg1, arg2, arg3);
                onInvokeCallback?.Invoke();
            }
        }
        
        public static void Invoke<T1, T2, T3, T4>(CustomEventType eventType, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, Action onInvokeCallback = null)
        {
            if (_events.TryGetValue(eventType, out var del))
            {
                (del as Action<T1, T2, T3, T4>)?.Invoke(arg1, arg2, arg3, arg4);
                onInvokeCallback?.Invoke();
            }
        }
        
        public static void Invoke<T1, T2, T3, T4, T5>(CustomEventType eventType, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, T5 arg5, Action onInvokeCallback = null)
        {
            if (_events.TryGetValue(eventType, out var del))
            {
                (del as Action<T1, T2, T3, T4, T5>)?.Invoke(arg1, arg2, arg3, arg4, arg5);
                onInvokeCallback?.Invoke();
            }
        }
        
        public static void Invoke<T1, T2, T3, T4, T5, T6>(CustomEventType eventType, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, T5 arg5, T6 arg6, Action onInvokeCallback = null)
        {
            if (_events.TryGetValue(eventType, out var del))
            {
                (del as Action<T1, T2, T3, T4, T5, T6>)?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
                onInvokeCallback?.Invoke();
            }
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
