using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.PolymorphicValueSystem.Compare
{
    public class BoolValueCompare : MonoBehaviour
    {
        #region Fields

        [SerializeField, PropertyOrder(-1)] private bool _checkOnStart;
        [SerializeReference] private PolymorphicValue<bool> _compareValue;
        [SerializeReference] private PolymorphicValue<bool> _compareValueTo;

        [FoldoutGroup("Events"), PropertyOrder(100)]
        [SerializeField] private UnityEvent OnValuesEqual;
        [FoldoutGroup("Events"), PropertyOrder(100)]
        [SerializeField] private UnityEvent OnValuesNotEqual;

        #endregion

        #region Public

        public void CheckValue(bool value)
        {
            bool valueTo = _compareValueTo.Value;
            
            if (value == valueTo)
                OnValuesEqual?.Invoke();
            else
                OnValuesNotEqual?.Invoke();
        }

        public void CheckValues()
        {
            bool valueA = _compareValue.Value;
            bool valueB = _compareValueTo.Value;
            
            CheckValue(valueA);
        }

        #endregion

        #region Monobehaviour

        private void Start()
        {
            if (_checkOnStart)
                CheckValues();
        }

        #endregion
    }
}
