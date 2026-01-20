using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class Vector2Value : PolymorphicValue<Vector2> { }

    [System.Serializable]
    public sealed class Vector2ConstantValue : Vector2Value
    {
        #region Fields

        [SerializeField] private Vector2 _value;

        #endregion

        #region Properties

        public override Vector2 Value
        {
            get => _value;
            set => _value = value;
        }

        #endregion
    }

    [System.Serializable]
    public sealed class Vector2ScriptableVariableValue : Vector2Value
    {
        #region Fields

        [SerializeField] private Vector2ScriptableVariable _variable;

        #endregion

        #region Properties

        public override Vector2 Value
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

        public Vector2ScriptableVariable Variable => _variable;

        #endregion
    }
}