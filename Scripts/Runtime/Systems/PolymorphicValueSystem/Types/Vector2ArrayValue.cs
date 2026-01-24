using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class Vector2ArrayValue : PolymorphicValue<Vector2[]> { }

    [System.Serializable]
    public sealed class Vector2ArrayConstantValue : Vector2ArrayValue
    {
        #region Fields

        [SerializeField] private Vector2[] _value;

        #endregion

        #region Properties

        public override Vector2[] Value
        {
            get => _value;
            set => _value = value;
        }

        #endregion

        #region Cloning

        public override PolymorphicValue<Vector2[]> Clone()
        {
            return new Vector2ArrayConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class Vector2ArrayScriptableVariableValue : Vector2ArrayValue
    {
        #region Fields

        [SerializeField] private Vector2ArrayScriptableVariable _variable;

        #endregion

        #region Properties

        public override Vector2[] Value
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

        public Vector2ArrayScriptableVariable Variable => _variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<Vector2[]> Clone()
        {
            return new Vector2ArrayScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
