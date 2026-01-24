using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class DoubleArrayValue : PolymorphicValue<double[]> { }

    [System.Serializable]
    public sealed class DoubleArrayConstantValue : DoubleArrayValue
    {
        #region Fields

        [SerializeField] private double[] _value;

        #endregion

        #region Properties

        public override double[] Value
        {
            get => _value;
            set => _value = value;
        }

        #endregion

        #region Cloning

        public override PolymorphicValue<double[]> Clone()
        {
            return new DoubleArrayConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class DoubleArrayScriptableVariableValue : DoubleArrayValue
    {
        #region Fields

        [SerializeField] private DoubleArrayScriptableVariable _variable;

        #endregion

        #region Properties

        public override double[] Value
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

        public DoubleArrayScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<double[]> Clone()
        {
            return new DoubleArrayScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
