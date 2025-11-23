using D_Dev.ScriptableVaiables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.ScriptableVaiables.Listeners
{
    public abstract class BaseScriptableVariableListener<T> : MonoBehaviour
    {
        #region Fields

        [SerializeField] private BaseScriptableVariable<T> _variable;
        
        [FoldoutGroup("Events")]
        public UnityEvent<T> OnValueChanged;

        #endregion

        #region Monobehaviour

        private void Awake() => _variable.OnValueUpdate += OnVariableValueChanged;
        private void OnDestroy() => _variable.OnValueUpdate -= OnVariableValueChanged;

        #endregion

        #region Listeners

        protected virtual void OnVariableValueChanged(T value)
        {
            OnValueChanged?.Invoke(value);
        }

        #endregion
    }
}
