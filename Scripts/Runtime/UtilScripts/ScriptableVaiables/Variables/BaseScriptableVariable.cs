using System;
using System.Collections.Generic;
using UnityEngine;

namespace D_Dev.ScriptableVaiables
{
    public class BaseScriptableVariable<T> : ScriptableObject
    {
        #region Fields

        [SerializeField] private T _value;
        [SerializeField] private bool _resetOnEnterRuntime;

        public event Action<T> OnValueUpdate;

        #endregion
        
        #region Properties

        public T Value
        {
            get => _value;
            set
            {
                if(EqualityComparer<T>.Default.Equals(_value, value))
                    return;

                _value = value;
                OnValueUpdate?.Invoke(_value);
            }
        }

        public bool ResetOnEnterRuntime => _resetOnEnterRuntime;

        #endregion

        #region ScriptableObject

        protected virtual void OnEnable()
        {
            if (_resetOnEnterRuntime)
                Value = default;
        }

        #endregion
    }
}
