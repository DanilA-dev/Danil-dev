using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class BoolArrayValue : PolymorphicValue<bool[]> { }

    [System.Serializable]
    public sealed class BoolArrayConstantValue : BoolArrayValue
    {
        #region Fields

        [SerializeField] private bool[] _value;

        #endregion

        #region Properties

        public override bool[] Value
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

        public override PolymorphicValue<bool[]> Clone()
        {
            return new BoolArrayConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class BoolArrayScriptableVariableValue : BoolArrayValue
    {
        #region Fields

        [SerializeField] private BoolArrayScriptableVariable _variable;

        #endregion

        #region Properties

        public override bool[] Value
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

        public BoolArrayScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<bool[]> Clone()
        {
            return new BoolArrayScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
