using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.PolymorphicValueSystem.Compare
{
    public class StringValueCompare : MonoBehaviour
    {
        #region Fields

        [SerializeField, PropertyOrder(-1)] private bool _checkOnStart;
        [SerializeReference] private PolymorphicValue<string> _compareValue;
        [SerializeReference] private PolymorphicValue<string> _compareValueTo;

        [FoldoutGroup("Events"), PropertyOrder(100)]
        [SerializeField] private UnityEvent OnValuesEqual;
        [FoldoutGroup("Events"), PropertyOrder(100)]
        [SerializeField] private UnityEvent OnValuesNotEqual;

        #endregion

        #region Public

        public void CheckValue(string value)
        {
            string valueTo = _compareValueTo.Value;
            
            if (value == valueTo)
                OnValuesEqual?.Invoke();
            else
                OnValuesNotEqual?.Invoke();
        }

        public void CheckValues()
        {
            string valueA = _compareValue.Value;
            string valueB = _compareValueTo.Value;
            
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
