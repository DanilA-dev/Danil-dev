using System;
using UnityEngine;

namespace D_Dev.UtilScripts.ScriptableVaiables
{
    public class BaseScriptableVariable<T> : ScriptableObject
    {
        #region Fields

        [SerializeField] private T _variable;

        public event Action<T> OnVariableSet;
        
        #endregion

        #region Properties

        public T Variable
        {
            get => _variable;
            set
            {
                _variable = value;
                OnVariableSet?.Invoke(_variable);
            }
        }

        #endregion
    }
}