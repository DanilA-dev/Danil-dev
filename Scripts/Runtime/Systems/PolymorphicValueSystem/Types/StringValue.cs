using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class StringValue : PolymorphicValue<string> { }

    [System.Serializable]
    public sealed class StringConstantValue : StringValue
    {
        #region Fields

        [SerializeField] private string _value;

        #endregion

        #region Properties

        public override string Value
        {
            get => _value;
            set
            {
                var oldValue = _value;
                _value = value;
                RaiseOnValueChanged(oldValue, value);
            }
        }

        #endregion

        #region Cloning

        public override PolymorphicValue<string> Clone()
        {
            return new StringConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class StringScriptableVariableValue : StringValue
    {
        #region Fields

        [SerializeField] private StringScriptableVariable _variable;

        #endregion

        #region Properties

        public override string Value
        {
            get
            {
                return _variable != null ? _variable.Value : default;
            }
            set
            {
                if (_variable == null) return;
                
                var oldValue = _variable.Value;
                _variable.Value = value;
                RaiseOnValueChanged(oldValue, value);
            }
        }

        public StringScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<string> Clone()
        {
            return new StringScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
