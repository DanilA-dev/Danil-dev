using System;
using System.Collections.Generic;
using D_Dev.PolymorphicValueSystem;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.EntityVariable.Types
{
    [System.Serializable]
    public abstract class PolymorphicEntityVariable<TPolymorphicValue, TValue> : BaseEntityVariable 
        where TPolymorphicValue : PolymorphicValue<TValue>
    {
        #region Fields

        [SerializeReference] protected TPolymorphicValue _value;
        
        public event Action<TValue> OnVariableChange;

        #endregion

        #region Properties

        public TPolymorphicValue Value
        {
            get => _value;
            set
            {
                var oldValue = _value.Value;
                _value.Value = value.Value;
                var newValue = _value.Value;
                if (!EqualityComparer<TValue>.Default.Equals(oldValue, newValue))
                {
                    OnVariableChange?.Invoke(_value.Value);
                }
            }
        }

        #endregion

        #region Constructor

        protected PolymorphicEntityVariable() {}
        
        protected PolymorphicEntityVariable(TPolymorphicValue value) { _value = value; }

        protected PolymorphicEntityVariable(StringScriptableVariable variableID, TPolymorphicValue value)
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
            Value = (TPolymorphicValue)value;
        }

        #endregion
    }
}
