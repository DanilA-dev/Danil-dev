using D_Dev.EntityVariable.Types;
using D_Dev.PolymorphicValueSystem;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.RuntimeEntityVariables.Extensions
{
    [System.Serializable]
    public class Vector2RuntimeVariableValue : Vector2Value
    {
        #region Fields

        [SerializeField] private StringScriptableVariable _variableID;
        [SerializeField] private RuntimeEntityVariablesContainer _runtimeEntityVariablesContainer;

        #endregion
        
        public override Vector2 Value
        {
            get
            {
                var variable = _runtimeEntityVariablesContainer.GetVariable<Vector2EntityVariable>(_variableID);
                return variable != null ? variable.Value.Value : default;
            }
            set => _runtimeEntityVariablesContainer?.SetValue(_variableID, new Vector2EntityVariable(_variableID, new Vector2ConstantValue { Value = value }));
        }
        public override PolymorphicValue<Vector2> Clone()
        {
            return new Vector2RuntimeVariableValue { _variableID = _variableID,
                _runtimeEntityVariablesContainer = _runtimeEntityVariablesContainer };
        }
    }
}