using System;
using System.Collections.Generic;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.EntityVariable
{
    [System.Serializable]
    public abstract class BaseEntityVariable
    {
        #region Fields

        [SerializeField] protected StringScriptableVariable _variableID;
        
        public event Action OnChange;

        #endregion

        #region Properties

        public StringScriptableVariable VariableID => _variableID;

        #endregion

        #region Abstract Methods

        public abstract object GetValueRaw();
        public abstract void SetValueRaw(object value);
        public abstract BaseEntityVariable Clone();

        #endregion
    }

    [System.Serializable]
    public abstract class EntityVariable<T> : BaseEntityVariable
    {
        #region Fields

        [SerializeField] protected T _value;
        
        public event Action<T> OnVariableChange;

        #endregion

        #region Properties

        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    OnVariableChange?.Invoke(_value);
                }
            }
        }

        #endregion

        #region Constructor

        protected EntityVariable() {}
        
        protected EntityVariable(T value) { _value = value; }

        protected EntityVariable(StringScriptableVariable variableID, T value)
        {
            _variableID = variableID;
            _value = value;
        }

        #endregion
        
        #region Overrides

        public override object GetValueRaw()
        {
            return Value;
        }

        public override void SetValueRaw(object value)
        {
            Value = (T)value;
        }

        #endregion
    }
}
