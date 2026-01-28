using D_Dev.EntityVariable.Types;
using D_Dev.PolymorphicValueSystem;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.RuntimeEntityVariables.Extensions
{
    [System.Serializable]
    public class BoolRuntimeVariableValue : BoolValue
    {
        #region Fields

        [SerializeField] private StringScriptableVariable _variableID;
        [SerializeField] private RuntimeEntityVariablesContainer _runtimeEntityVariablesContainer;

        #endregion
        
        public override bool Value
        {
            get
            {
                var variable = _runtimeEntityVariablesContainer.GetVariable<BoolEntityVariable>(_variableID);
                return variable != null ? variable.Value.Value : default;
            }
            set => _runtimeEntityVariablesContainer?.SetValue(_variableID, new BoolEntityVariable(_variableID, new BoolConstantValue { Value = value }));
        }
        public override PolymorphicValue<bool> Clone()
        {
            return new BoolRuntimeVariableValue { _variableID = _variableID,
                _runtimeEntityVariablesContainer = _runtimeEntityVariablesContainer };
        }
    }
}