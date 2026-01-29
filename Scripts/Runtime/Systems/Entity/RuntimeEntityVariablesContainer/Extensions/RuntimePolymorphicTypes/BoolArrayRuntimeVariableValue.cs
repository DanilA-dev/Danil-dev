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
        
        private BoolArrayEntityVariable _cachedVariable;

        #endregion

        #region Properties

        public override bool[] Value
        {
            get
            {
                if (_cachedVariable == null)
                    _cachedVariable = _runtimeEntityVariablesContainer?.GetVariable<BoolArrayEntityVariable>(_variableID);
                
                if (_cachedVariable == null)
                    return new bool[0];
                
                return _cachedVariable.Value.Value;
            }
            set
            {
                if (_cachedVariable == null)
                    _cachedVariable = _runtimeEntityVariablesContainer?.GetVariable<BoolArrayEntityVariable>(_variableID);
                
                if (_cachedVariable != null)
                    _cachedVariable.Value.Value = value;
            }
        }

        #endregion

        #region Clone

        public override PolymorphicValue<bool[]> Clone()
        {
            return new BoolArrayRuntimeVariableValue
            {
                _variableID = _variableID,
                _runtimeEntityVariablesContainer = _runtimeEntityVariablesContainer
            };
        }

        #endregion
    }
}