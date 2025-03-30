using D_Dev.UtilScripts.ScriptableVaiables;
using UnityEngine;

namespace D_dev.Scripts.Runtime.UtilScripts.ScriptableVaiables.Attachers
{
    public class BaseScriptableVariableAttacher<TVariable, T> : MonoBehaviour where TVariable : BaseScriptableVariable<T>
    {
        #region Fields
        
        [SerializeField] private TVariable _variable;
        [SerializeField] private T _object;
        [Space]
        [SerializeField] private bool _attachOnAwake;
        [SerializeField] private bool _attachOnStart;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            if (_attachOnAwake)
                AttachObjectToVariable(_object);
        }

        private void Start()
        {
            if (_attachOnStart)
                AttachObjectToVariable(_object);
        }

        #endregion

        #region Public

        public void AttachObjectToVariable(T objectToAttach)
        {
            if(_variable == null)
                return;

            _variable.Variable = objectToAttach;
        }

        public void ResetVariable() => _variable.Variable = default;

        #endregion
    }
}