using D_Dev.Conditions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.PolymorphicValueSystem.Compare
{
    public class IntValueCompare : MonoBehaviour
    {
        #region Fields

        [SerializeField, PropertyOrder(-1)] private bool _checkOnStart;
        [SerializeReference] private PolymorphicValue<int> _compareValue;
        [SerializeReference] private PolymorphicValue<int> _compareValueTo;
        
        [SerializeField, PropertyOrder(-1)] private ValueCompareType _compareType = ValueCompareType.Equal;

        [FoldoutGroup("Events"), PropertyOrder(100)]
        [SerializeField] private UnityEvent OnValueLess;
        [FoldoutGroup("Events"), PropertyOrder(100)]
        [SerializeField] private UnityEvent OnValueEqual;
        [FoldoutGroup("Events"), PropertyOrder(100)]
        [SerializeField] private UnityEvent OnValueBigger;
        [FoldoutGroup("Events"), PropertyOrder(100)]
        [SerializeField] private UnityEvent OnValueEqualOrLess;
        [FoldoutGroup("Events"), PropertyOrder(100)]
        [SerializeField] private UnityEvent OnValueEqualOrBigger;
        [FoldoutGroup("Events"), PropertyOrder(100)]
        [SerializeField] private UnityEvent OnValuesEqual;
        [FoldoutGroup("Events"), PropertyOrder(100)]
        [SerializeField] private UnityEvent OnValuesNotEqual;

        #endregion

        #region Public

        public void CheckValue(int value)
        {
            int valueTo = _compareValueTo.Value;
            
            switch (_compareType)
            {
                case ValueCompareType.Less:
                    if (value < valueTo) OnValueLess?.Invoke();
                    break;
                case ValueCompareType.Equal:
                    if (value == valueTo) OnValueEqual?.Invoke();
                    break;
                case ValueCompareType.Bigger:
                    if (value > valueTo) OnValueBigger?.Invoke();
                    break;
                case ValueCompareType.EqualOrLess:
                    if (value <= valueTo) OnValueEqualOrLess?.Invoke();
                    break;
                case ValueCompareType.EqualOrBigger:
                    if (value >= valueTo) OnValueEqualOrBigger?.Invoke();
                    break;
            }
        }

        public void CheckValues()
        {
            int valueA = _compareValue.Value;
            int valueB = _compareValueTo.Value;
            
            CheckValue(valueA);
            
            if (valueA == valueB)
                OnValuesEqual?.Invoke();
            else
                OnValuesNotEqual?.Invoke();
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
