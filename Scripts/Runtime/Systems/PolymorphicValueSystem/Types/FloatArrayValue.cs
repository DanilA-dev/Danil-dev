using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class FloatArrayValue : PolymorphicValue<float[]> { }

    [System.Serializable]
    public sealed class FloatArrayConstantValue : FloatArrayValue
    {
        #region Fields

        [SerializeField] private float[] _value;

        #endregion

        #region Properties

        public override float[] Value
        {
            get => _value;
            set => _value = value;
        }

        #endregion
    }

    [System.Serializable]
    public sealed class FloatArrayScriptableVariableValue : FloatArrayValue
    {
        #region Fields

        [SerializeField] private FloatArrayScriptableVariable _variable;

        #endregion

        #region Properties

        public override float[] Value
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

        public FloatArrayScriptableVariable Variable => _variable;

        #endregion
    }
}