using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class Vector3Value : PolymorphicValue<Vector3> { }

    [System.Serializable]
    public sealed class Vector3ConstantValue : Vector3Value
    {
        #region Fields

        [SerializeField] private Vector3 _value;

        #endregion

        #region Properties

        public override Vector3 Value
        {
            get => _value;
            set => _value = value;
        }

        #endregion

        #region Cloning

        public override PolymorphicValue<Vector3> Clone()
        {
            return new Vector3ConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class Vector3ScriptableVariableValue : Vector3Value
    {
        #region Fields

        [SerializeField] private Vector3ScriptableVariable _variable;

        #endregion

        #region Properties

        public override Vector3 Value
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

        public Vector3ScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<Vector3> Clone()
        {
            return new Vector3ScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
