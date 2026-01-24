using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class BoolValue : PolymorphicValue<bool> { }

    [System.Serializable]
    public sealed class BoolConstantValue : BoolValue
    {
        #region Fields

        [SerializeField] private bool _value;

        #endregion

        #region Properties

        public override bool Value
        {
            get => _value;
            set => _value = value;
        }

        #endregion

        #region Cloning

        public override PolymorphicValue<bool> Clone()
        {
            return new BoolConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class BoolScriptableVariableValue : BoolValue
    {
        #region Fields

        [SerializeField] private BoolScriptableVariable _variable;

        #endregion

        #region Properties

        public override bool Value
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

        public BoolScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<bool> Clone()
        {
            return new BoolScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
