using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class TransformArrayValue : PolymorphicValue<Transform[]> { }

    [System.Serializable]
    public sealed class TransformArrayConstantValue : TransformArrayValue
    {
        #region Fields

        [SerializeField] private Transform[] _value;

        #endregion

        #region Properties

        public override Transform[] Value
        {
            get => _value;
            set => _value = value;
        }

        #endregion

        #region Cloning

        public override PolymorphicValue<Transform[]> Clone()
        {
            return new TransformArrayConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class TransformArrayScriptableVariableValue : TransformArrayValue
    {
        #region Fields

        [SerializeField] private TransformArrayScriptableVariable _variable;

        #endregion

        #region Properties

        public override Transform[] Value
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

        public TransformArrayScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<Transform[]> Clone()
        {
            return new TransformArrayScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
