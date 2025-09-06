using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_dev.Scripts.Runtime.UtilScripts.CurrencySystem
{
    public class CurrencyInfoCompareEvents : MonoBehaviour
    {
        #region Enums
        
        public enum ComparisonType
        {
            WithValue,
            WithMaxValue,
            WithCurrencyItem
        }
        
        #endregion
        
        #region Classes
        
        [Serializable]
        public class ComparisonEvents
        {
            [SerializeField] private UnityEvent _onLessThan;
            [SerializeField] private UnityEvent _onEqualTo;
            [SerializeField] private UnityEvent _onGreaterThan;
            [SerializeField] private UnityEvent _onLessOrEqualTo;
            [SerializeField] private UnityEvent _onGreaterOrEqualTo;
            
            public UnityEvent OnLessThan => _onLessThan;
            public UnityEvent OnEqualTo => _onEqualTo;
            public UnityEvent OnGreaterThan => _onGreaterThan;
            public UnityEvent OnLessOrEqualTo => _onLessOrEqualTo;
            public UnityEvent OnGreaterOrEqualTo => _onGreaterOrEqualTo;
        }
        
        #endregion
        
        #region Fields
        
        [Title("General")]
        [SerializeField] private CurrencyInfo _currencyToCheck;
        [SerializeField] private ComparisonType _comparisonType;
        [SerializeField] private bool _invokeOnStart;
        
        [Title("Value comparison")]
        [ShowIf("@_comparisonType == ComparisonType.WithValue")]
        [SerializeField] private int _valueToCompare;
        
        [Title("Currency comparison")]
        [ShowIf("@_comparisonType == ComparisonType.WithCurrencyItem")]
        [SerializeField] private CurrencyInfo _otherCurrencyItem;
        
        [FoldoutGroup("Events")]
        [ShowIf("@_comparisonType == ComparisonType.WithValue")]
        [SerializeField] private ComparisonEvents _valueComparisonEvents;
        
        [FoldoutGroup("Events")]
        [ShowIf("@_comparisonType == ComparisonType.WithMaxValue")]
        [SerializeField] private ComparisonEvents _maxValueComparisonEvents;
        
        [FoldoutGroup("Events")]
        [ShowIf("@_comparisonType == ComparisonType.WithCurrencyItem")]
        [SerializeField] private ComparisonEvents _currencyComparisonEvents;
        
        #endregion
        
        #region Properties
        
        public CurrencyInfo CurrencyToCheck => _currencyToCheck;
        public ComparisonType CurrentComparisonType => _comparisonType;
        public int ValueToCompare => _valueToCompare;
        public CurrencyInfo OtherCurrencyItem => _otherCurrencyItem;
        
        #endregion

        #region Monobehaviour

        private void Start()
        {
            if (_invokeOnStart)
                Compare();
        }

        #endregion
        
        #region Public Methods
        
        public void Compare()
        {
            if (_currencyToCheck == null)
            {
                Debug.LogError("[CurrencyCompareEvents] CurrencyToCheck is null!");
                return;
            }
            
            switch (_comparisonType)
            {
                case ComparisonType.WithValue:
                    CompareWithValue();
                    break;
                case ComparisonType.WithMaxValue:
                    CompareWithMaxValue();
                    break;
                case ComparisonType.WithCurrencyItem:
                    CompareWithCurrencyItem();
                    break;
            }
        }
       
        public void CompareWithValue(int numberToCompare)
        {
            _valueToCompare = numberToCompare;
            CompareWithValue();
        }
        
        public void CompareWithCurrencyItem(CurrencyInfo otherCurrency)
        {
            _otherCurrencyItem = otherCurrency;
            CompareWithCurrencyItem();
        }
        
        #endregion
        
        #region Private Methods
        
        private void CompareWithValue()
        {
            if (_currencyToCheck == null)
            {
                Debug.LogError("[CurrencyCompareEvents] CurrencyToCheck is null!");
                return;
            }
            
            int currentValue = _currencyToCheck.Currency.Value;
            int compareValue = _valueToCompare;
            
            PerformComparison(currentValue, compareValue, _valueComparisonEvents, 
                _currencyToCheck.name, compareValue.ToString());
        }
        
        private void CompareWithMaxValue()
        {
            if (_currencyToCheck == null)
            {
                Debug.LogError("[CurrencyCompareEvents] CurrencyToCheck is null!");
                return;
            }
            
            if (!_currencyToCheck.Currency.HasMaxValue)
            {
                Debug.Log($"[CurrencyCompareEvents] {_currencyToCheck.name} не имеет максимального значения!");
                return;
            }
            
            int currentValue = _currencyToCheck.Currency.Value;
            int maxValue = _currencyToCheck.Currency.MaxValue;
            
            PerformComparison(currentValue, maxValue, _maxValueComparisonEvents, 
                _currencyToCheck.name, $"MaxValue ({maxValue})");
        }
        
        private void CompareWithCurrencyItem()
        {
            if (_currencyToCheck == null)
            {
                Debug.LogError("[CurrencyCompareEvents] CurrencyToCheck is null!");
                return;
            }
            
            if (_otherCurrencyItem == null)
            {
                Debug.LogError("[CurrencyCompareEvents] OtherCurrencyItem is null!");
                return;
            }
            
            int currentValue = _currencyToCheck.Currency.Value;
            int otherValue = _otherCurrencyItem.Currency.Value;
            
            PerformComparison(currentValue, otherValue, _currencyComparisonEvents, 
                _currencyToCheck.name, $"{_otherCurrencyItem.name} ({otherValue})");
        }
        
        private void PerformComparison(int currentValue, int compareValue, ComparisonEvents events, 
            string currentName, string compareName)
        {
            if (currentValue < compareValue)
            {
                Debug.Log($"[CurrencyCompareEvents] {currentName} ({currentValue}) < {compareName}");
                events.OnLessThan?.Invoke();
            }
            else if (currentValue == compareValue)
            {
                Debug.Log($"[CurrencyCompareEvents] {currentName} ({currentValue}) = {compareName}");
                events.OnEqualTo?.Invoke();
            }
            else
            {
                Debug.Log($"[CurrencyCompareEvents] {currentName} ({currentValue}) > {compareName}");
                events.OnGreaterThan?.Invoke();
            }
            
            if (currentValue <= compareValue)
            {
                Debug.Log($"[CurrencyCompareEvents] {currentName} ({currentValue}) <= {compareName}");
                events.OnLessOrEqualTo?.Invoke();
            }
            
            if (currentValue >= compareValue)
            {
                Debug.Log($"[CurrencyCompareEvents] {currentName} ({currentValue}) >= {compareName}");
                events.OnGreaterOrEqualTo?.Invoke();
            }
        }
        
        #endregion
        
        #region Debug
        
        [PropertySpace(10)]
        [Button()]
        [ShowIf("@_comparisonType == ComparisonType.WithValue")]
        public void DebugCompareWithValue()
        {
            CompareWithValue();
        }
        
        [Button()]
        [ShowIf("@_comparisonType == ComparisonType.WithMaxValue")]
        public void DebugCompareWithMaxValue()
        {
            CompareWithMaxValue();
        }
        
        [Button()]
        [ShowIf("@_comparisonType == ComparisonType.WithCurrencyItem")]
        public void DebugCompareWithCurrency()
        {
            CompareWithCurrencyItem();
        }
        
        #endregion
    }
}