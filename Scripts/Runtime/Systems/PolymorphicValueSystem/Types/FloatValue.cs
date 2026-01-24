using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class FloatValue : PolymorphicValue<float> { }

    [System.Serializable]
    public sealed class FloatConstantValue : FloatValue
    {
        #region Fields

        [SerializeField] private float _value;

        #endregion

        #region Properties

        public override float Value
        {
            get => _value;
            set => _value = value;
        }

        #endregion

        #region Cloning

        public override PolymorphicValue<float> Clone()
        {
            return new FloatConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class FloatScriptableVariableValue : FloatValue
    {
        #region Fields

        [SerializeField] private FloatScriptableVariable _variable;

        #endregion

        #region Properties

        public override float Value
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

        public FloatScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<float> Clone()
        {
            return new FloatScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
