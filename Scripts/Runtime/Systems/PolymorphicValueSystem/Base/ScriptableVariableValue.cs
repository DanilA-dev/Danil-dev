using System;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public sealed class ScriptableVariableValue<T, TVariable> : PolymorphicValue<T>
        where TVariable : BaseScriptableVariable<T>
    {
        #region Fields

        [SerializeField] private TVariable _variable;

        #endregion

        #region Properties

        public TVariable Variable => _variable;

        public override T Value
        {
            get
            {
                return _variable != null ? _variable.Value : default;
            }
            set
            {
                if (_variable == null)
                    return;

                _variable.Value = value;
            }
        }

        #endregion

        #region Constructor

        public ScriptableVariableValue() { }
        public ScriptableVariableValue(TVariable variable) => _variable = variable;

        #endregion

        #region Cloning

        public override PolymorphicValue<T> Clone()
        {
            return new ScriptableVariableValue<T, TVariable>(_variable);
        }

        #endregion
    }
}
