using D_Dev.EntityVariable.Types;
using D_Dev.PolymorphicValueSystem;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.RuntimeEntityVariables.Extensions
{
    [System.Serializable]
    public class FloatRuntimeVariableValue : FloatValue
    {
        #region Fields

        [SerializeField] private StringScriptableVariable _variableID;
        [SerializeField] private RuntimeEntityVariablesContainer _runtimeEntityVariablesContainer;

        #endregion
        
        public override float Value
        {
            get
            {
                var variable = _runtimeEntityVariablesContainer.GetVariable<FloatEntityVariable>(_variableID);
                return variable != null ? variable.Value.Value : default;
            }
            set => _runtimeEntityVariablesContainer?.SetValue(_variableID, new FloatEntityVariable(_variableID, new FloatConstantValue { Value = value }));
        }
        public override PolymorphicValue<float> Clone()
        {
            return new FloatRuntimeVariableValue { _variableID = _variableID,
                _runtimeEntityVariablesContainer = _runtimeEntityVariablesContainer };
        }
    }
}