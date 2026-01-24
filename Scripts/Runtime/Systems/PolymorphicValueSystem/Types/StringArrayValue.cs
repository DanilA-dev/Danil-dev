using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class StringArrayValue : PolymorphicValue<string[]> { }

    [System.Serializable]
    public sealed class StringArrayConstantValue : StringArrayValue
    {
        #region Fields

        [SerializeField] private string[] _value;

        #endregion

        #region Properties

        public override string[] Value
        {
            get => _value;
            set => _value = value;
        }

        #endregion

        #region Cloning

        public override PolymorphicValue<string[]> Clone()
        {
            return new StringArrayConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class StringArrayScriptableVariableValue : StringArrayValue
    {
        #region Fields

        [SerializeField] private StringArrayScriptableVariable _variable;

        #endregion

        #region Properties

        public override string[] Value
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

        public StringArrayScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<string[]> Clone()
        {
            return new StringArrayScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
