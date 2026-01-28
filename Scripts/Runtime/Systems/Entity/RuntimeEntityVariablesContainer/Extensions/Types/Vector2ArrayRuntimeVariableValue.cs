using D_Dev.EntityVariable.Types;
using D_Dev.PolymorphicValueSystem;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.RuntimeEntityVariables.Extensions
{
    [System.Serializable]
    public class Vector2ArrayRuntimeVariableValue : Vector2ArrayValue
    {
        #region Fields

        [SerializeField] private StringScriptableVariable _variableID;
        [SerializeField] private RuntimeEntityVariablesContainer _runtimeEntityVariablesContainer;

        #endregion
        
        public override Vector2[] Value
        {
            get
            {
                var variable = _runtimeEntityVariablesContainer.GetVariable<Vector2ArrayEntityVariable>(_variableID);
                return variable != null ? variable.Value.Value : default;
            }
            set => _runtimeEntityVariablesContainer?.SetValue(_variableID, new Vector2ArrayEntityVariable(_variableID, new Vector2ArrayConstantValue { Value = value }));
        }
        public override PolymorphicValue<Vector2[]> Clone()
        {
            return new Vector2ArrayRuntimeVariableValue { _variableID = _variableID,
                _runtimeEntityVariablesContainer = _runtimeEntityVariablesContainer };
        }
    }
}