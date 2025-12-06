using D_Dev.ValueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.Utility
{
    public class BoolInverterBehaviour : MonoBehaviour
    {
        #region Fields

        [SerializeField] private BoolValue _value;
        [Space]
        [FoldoutGroup("Events")]
        public UnityEvent OnTrue;
        [FoldoutGroup("Events")]
        public UnityEvent OnFalse;

        #endregion

        #region Public

        public void Invert()
        {
            _value.Value = !_value.Value;
            if (_value.Value)
                OnTrue.Invoke();
            else
                OnFalse.Invoke();
        }
        
        public void Invert(bool value)
        {
            value = !value;
            if (value)
                OnTrue.Invoke();
            else
                OnFalse.Invoke();
        }

        #endregion
    }
}
