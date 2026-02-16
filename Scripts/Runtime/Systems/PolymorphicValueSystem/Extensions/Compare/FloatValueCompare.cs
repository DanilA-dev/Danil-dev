using D_Dev.Conditions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.PolymorphicValueSystem.Compare
{
    public class FloatValueCompare : MonoBehaviour
    {
        #region Fields

        [SerializeField, PropertyOrder(-1)] private bool _checkOnStart;
        [SerializeReference] private PolymorphicValue<float> _compareValue;
        [SerializeReference] private PolymorphicValue<float> _compareValueTo;
        
        [SerializeField, PropertyOrder(-1)] private ValueCompareType _compareType = ValueCompareType.Equal;

        [FoldoutGroup("Events"), PropertyOrder(100)]
        [SerializeField] private UnityEvent OnValueCompareTrue;

        #endregion

        #region Public

        public void CheckValue(float value)
        {
            float valueTo = _compareValueTo.Value;
            
            bool isTrue = _compareType switch
            {
                ValueCompareType.Less => value < valueTo,
                ValueCompareType.Equal => Mathf.Approximately(value, valueTo),
                ValueCompareType.Bigger => value > valueTo,
                ValueCompareType.EqualOrLess => value <= valueTo,
                ValueCompareType.EqualOrBigger => value >= valueTo,
                _ => false
            };

            if (isTrue) OnValueCompareTrue?.Invoke();
        }

        public void CheckValues()
        {
            float valueA = _compareValue.Value;
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
