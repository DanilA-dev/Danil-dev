using D_Dev.EntityVariable.Types;
using D_Dev.PolymorphicValueSystem;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.RuntimeEntityVariables.Extensions
{
    [System.Serializable]
    public class BoolArrayRuntimeVariableValue : BoolArrayValue
    {
        #region Fields

        [SerializeField] private StringScriptableVariable _variableID;
        [SerializeField] private RuntimeEntityVariablesContainer _runtimeEntityVariablesContainer;

        #endregion
        
        public override bool[] Value
        {
            get
            {
                var variable = _runtimeEntityVariablesContainer.GetVariable<BoolArrayEntityVariable>(_variableID);
                return variable != null ? variable.Value.Value : default;
            }
            set => _runtimeEntityVariablesContainer?.SetValue(_variableID, new BoolArrayEntityVariable(_variableID, new BoolArrayConstantValue { Value = value }));
        }
        public override PolymorphicValue<bool[]> Clone()
        {
            return new BoolArrayRuntimeVariableValue { _variableID = _variableID,
                _runtimeEntityVariablesContainer = _runtimeEntityVariablesContainer };
        }
    }
}