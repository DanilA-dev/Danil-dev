using UnityEngine;

namespace D_Dev.ScriptableVaiables.Setters
{
    public class BaseScriptableVariableSetter<T, TVariable> : MonoBehaviour where TVariable : BaseScriptableVariable<T>
    {
        #region Fields

        [SerializeField] private TVariable _variable;
        [SerializeField] private T _value;
        [Space]
        [SerializeField] private bool _setOnAwake;
        [SerializeField] private bool _setOnStart;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            if (_setOnAwake)
                SetValue(_value);
        }

        private void Start()
        {
            if (_setOnStart)
                SetValue(_value);
        }

        #endregion
        
        #region Public

        public void SetValue(T value)
        {
            if(value == null)
                return;
            
            _variable.Value = value;
        }

        #endregion
    }
}