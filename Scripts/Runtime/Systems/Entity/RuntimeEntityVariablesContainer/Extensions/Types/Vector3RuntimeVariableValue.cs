using D_Dev.EntityVariable.Types;
using D_Dev.PolymorphicValueSystem;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.RuntimeEntityVariables.Extensions
{
    [System.Serializable]
    public class Vector3RuntimeVariableValue : Vector3Value
    {
        #region Fields

        [SerializeField] private StringScriptableVariable _variableID;
        [SerializeField] private RuntimeEntityVariablesContainer _runtimeEntityVariablesContainer;

        #endregion
        
        public override Vector3 Value
        {
            get
            {
                var variable = _runtimeEntityVariablesContainer.GetVariable<Vector3EntityVariable>(_variableID);
                return variable != null ? variable.Value.Value : default;
            }
            set => _runtimeEntityVariablesContainer?.SetValue(_variableID, new Vector3EntityVariable(_variableID, new Vector3ConstantValue { Value = value }));
        }
        public override PolymorphicValue<Vector3> Clone()
        {
            return new Vector3RuntimeVariableValue { _variableID = _variableID,
                _runtimeEntityVariablesContainer = _runtimeEntityVariablesContainer };
        }
    }
}