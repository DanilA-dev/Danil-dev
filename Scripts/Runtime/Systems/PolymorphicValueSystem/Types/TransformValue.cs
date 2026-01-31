using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class TransformValue : PolymorphicValue<Transform> { }

    [System.Serializable]
    public sealed class TransformConstantValue : TransformValue
    {
        #region Fields

        [SerializeField] private Transform _value;

        #endregion

        #region Properties

        public override Transform Value
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

        public override PolymorphicValue<Transform> Clone()
        {
            return new TransformConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class TransformScriptableVariableValue : TransformValue
    {
        #region Fields

        [SerializeField] private TransformScriptableVariable _variable;

        #endregion

        #region Properties

        public override Transform Value
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

        public TransformScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<Transform> Clone()
        {
            return new TransformScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
