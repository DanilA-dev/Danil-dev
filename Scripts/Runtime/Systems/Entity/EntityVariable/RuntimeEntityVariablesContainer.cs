using System;
using System.Collections.Generic;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.EntityVariable
{
    public class RuntimeEntityVariablesContainer : MonoBehaviour
    {
        #region Fields

        [SerializeReference] private List<BaseEntityVariable> _variables = new();

        private Dictionary<StringScriptableVariable, BaseEntityVariable> _variableMap;
        
        public event Action OnInitialized;
            
        #endregion

        #region Public

        public void Init(List<BaseEntityVariable> variablesFromInfo)
        {
            _variableMap = new();
            _variables.Clear();
            
            if(variablesFromInfo == null || variablesFromInfo.Count == 0)
                return;

            foreach (var variable in variablesFromInfo)
            {
                if(variable == null)
                    continue;
                
                var clonedVar = variable.Clone();
                _variables.Add(clonedVar);
                _variableMap.TryAdd(variable.VariableID, clonedVar);
            }
            OnInitialized?.Invoke();
        }

        public T GetVariable<T>(StringScriptableVariable variableID) where T : BaseEntityVariable
        {
            if (_variableMap.TryGetValue(variableID, out var variable))
            {
                return variable as T;
            }
        
            Debug.LogError($"Variable '{variableID}' not found in RuntimeEntityVariablesContainer on GameObject {gameObject.name}");
            return null;
        }

        public T GetValue<T>(StringScriptableVariable variableID) where T : BaseEntityVariable
        {
            var variable = GetVariable<T>(variableID);
            if (variable != null)
                return variable.GetValueRaw() as T;
            
            return null;
        }

        public void SetValue<T>(StringScriptableVariable variableID, T value) where T : BaseEntityVariable
        {
            var variable = GetVariable<T>(variableID);
            if (variable != null)
                variable.SetValueRaw(value);
        }
        
        #endregion
    }
}