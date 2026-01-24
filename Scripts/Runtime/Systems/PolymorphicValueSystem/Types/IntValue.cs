using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class IntValue : PolymorphicValue<int> { }

    [System.Serializable]
    public sealed class IntConstantValue : IntValue
    {
        #region Fields

        [SerializeField] private int _value;

        #endregion

        #region Properties

        public override int Value
        {
            get => _value;
            set => _value = value;
        }

        #endregion

        #region Cloning

        public override PolymorphicValue<int> Clone()
        {
            return new IntConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class IntScriptableVariableValue : IntValue
    {
        #region Fields

        [SerializeField] private IntScriptableVariable _variable;

        #endregion

        #region Properties

        public override int Value
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

        public IntScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<int> Clone()
        {
            return new IntScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
