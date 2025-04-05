using System;
using UnityEngine;

namespace D_Dev.UtilScripts.ScriptableVaiables
{
    public class BaseScriptableVariable<T> : ScriptableObject
    {
        #region Fields

        [SerializeField] private T _variable;

        public event Action<T> OnVariableUpdate;
        
        #endregion

        #region Properties

        public T Variable
        {
            get => _variable;
            set
            {
                _variable = value;
                OnVariableUpdate?.Invoke(_variable);
            }
        }

        #endregion
    }
}