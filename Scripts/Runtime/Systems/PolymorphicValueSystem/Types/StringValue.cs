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
            set => _value = value;
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
                if (_variable != null)
                    _variable.Value = value;
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
