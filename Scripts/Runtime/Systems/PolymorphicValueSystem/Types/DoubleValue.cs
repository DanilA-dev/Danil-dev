using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class DoubleValue : PolymorphicValue<double> { }

    [System.Serializable]
    public sealed class DoubleConstantValue : DoubleValue
    {
        #region Fields

        [SerializeField] private double _value;

        #endregion

        #region Properties

        public override double Value
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

        public override PolymorphicValue<double> Clone()
        {
            return new DoubleConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class DoubleScriptableVariableValue : DoubleValue
    {
        #region Fields

        [SerializeField] private DoubleScriptableVariable _variable;

        #endregion

        #region Properties

        public override double Value
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

        public DoubleScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<double> Clone()
        {
            return new DoubleScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
