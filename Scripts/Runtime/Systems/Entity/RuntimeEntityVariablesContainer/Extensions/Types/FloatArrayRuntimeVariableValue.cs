using D_Dev.EntityVariable.Types;
using D_Dev.PolymorphicValueSystem;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.RuntimeEntityVariables.Extensions
{
    [System.Serializable]
    public class FloatArrayRuntimeVariableValue : FloatArrayValue
    {
        #region Fields

        [SerializeField] private StringScriptableVariable _variableID;
        [SerializeField] private RuntimeEntityVariablesContainer _runtimeEntityVariablesContainer;

        #endregion
        
        public override float[] Value
        {
            get
            {
                var variable = _runtimeEntityVariablesContainer.GetVariable<FloatArrayEntityVariable>(_variableID);
                return variable != null ? variable.Value.Value : default;
            }
            set => _runtimeEntityVariablesContainer?.SetValue(_variableID, new FloatArrayEntityVariable(_variableID, new FloatArrayConstantValue { Value = value }));
        }
        public override PolymorphicValue<float[]> Clone()
        {
            return new FloatArrayRuntimeVariableValue { _variableID = _variableID,
                _runtimeEntityVariablesContainer = _runtimeEntityVariablesContainer };
        }
    }
}