using D_Dev.EntityVariable.Types;
using D_Dev.PolymorphicValueSystem;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.RuntimeEntityVariables.Extensions
{
    [System.Serializable]
    public class DoubleArrayRuntimeVariableValue : DoubleArrayValue
    {
        #region Fields

        [SerializeField] private StringScriptableVariable _variableID;
        [SerializeField] private RuntimeEntityVariablesContainer _runtimeEntityVariablesContainer;

        #endregion
        
        public override double[] Value
        {
            get
            {
                var variable = _runtimeEntityVariablesContainer.GetVariable<DoubleArrayEntityVariable>(_variableID);
                return variable != null ? variable.Value.Value : default;
            }
            set => _runtimeEntityVariablesContainer?.SetValue(_variableID, new DoubleArrayEntityVariable(_variableID, new DoubleArrayConstantValue { Value = value }));
        }
        public override PolymorphicValue<double[]> Clone()
        {
            return new DoubleArrayRuntimeVariableValue { _variableID = _variableID,
                _runtimeEntityVariablesContainer = _runtimeEntityVariablesContainer };
        }
    }
}