using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class IntArrayValue : PolymorphicValue<int[]> { }

    [System.Serializable]
    public sealed class IntArrayConstantValue : IntArrayValue
    {
        #region Fields

        [SerializeField] private int[] _value;

        #endregion

        #region Properties

        public override int[] Value
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

        public override PolymorphicValue<int[]> Clone()
        {
            return new IntArrayConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class IntArrayScriptableVariableValue : IntArrayValue
    {
        #region Fields

        [SerializeField] private IntArrayScriptableVariable _variable;

        #endregion

        #region Properties

        public override int[] Value
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

        public IntArrayScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<int[]> Clone()
        {
            return new IntArrayScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
