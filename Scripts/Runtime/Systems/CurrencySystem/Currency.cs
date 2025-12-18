using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.CurrencySystem
{
    [System.Serializable]
    public class Currency
    {
        #region Enums

        public enum CurrencyActionType
        {
            Deposit = 0,
            Withdraw = 1,
            Set = 2,
        }
        
        #endregion
        
        #region Classes

        public struct CurrencyEvent
        {
            public CurrencyActionType ActionType;
            public bool IsSuccess;
        }

        #endregion
        
        #region Fields

        [SerializeField] private string _name;
        [Space]
        [SerializeField, ReadOnly] private long _value;
        [SerializeField] private long _defaultValue;
        [SerializeField] private bool _hasMaxValue;
        [ShowIf(nameof(_hasMaxValue))]
        [SerializeField] private long _maxValue;

        public event Action<CurrencyEvent,long> OnCurrencyUpdate;
            
        #endregion

        #region Properties
        public decimal Value => (decimal)_value / 100m;

        public bool HasMaxValue
        {
            get => _hasMaxValue;
            set => _hasMaxValue = value;
        }


        public decimal MaxValue
        {
            get => (decimal)_maxValue / 100m;
            set => _maxValue = (long)(value * 100m);
        }

        public decimal DefaultValue
        {
            get => (decimal)_defaultValue / 100m;
            set => _defaultValue = (long)(value * 100m);
        }

        #endregion

        #region Constructors

        public Currency(string name, decimal initialValue)
        {
            _name = name;
            _value = (long)(initialValue * 100m);
        }

        #endregion
        
        #region Public

        public bool TryDeposit(decimal depositValue)
        {
            long depositInCents = (long)(depositValue * 100m);
            if (depositValue <= 0 || _hasMaxValue && _value >= _maxValue)
            {
                OnCurrencyUpdate?.Invoke(new CurrencyEvent
                {
                    ActionType = CurrencyActionType.Deposit,
                    IsSuccess = false,
                }, _value);
                Debug.Log($"[Currency : <color=pink>{_name}</color>] Deposit - {depositValue}, <color=red> Failed </color>");
                return false;
            }

            var newValue = _value + depositInCents;
            if (_hasMaxValue && newValue >= _maxValue)
                _value = _maxValue;
            else
                _value += depositInCents;

            OnCurrencyUpdate?.Invoke(new CurrencyEvent{ActionType = CurrencyActionType.Deposit, IsSuccess = true }, _value);
            Debug.Log($"[Currency : <color=pink>{_name}</color>] Deposit - {depositValue}, <color=green> Success </color>");
            return true;
        }

        public bool TryWithdraw(decimal withdrawValue)
        {
            long withdrawInCents = (long)(withdrawValue * 100m);
            if (withdrawValue <= 0 || _value < withdrawInCents)
            {
                OnCurrencyUpdate?.Invoke(new CurrencyEvent
                {
                    ActionType = CurrencyActionType.Withdraw, IsSuccess = false
                }, _value);
                Debug.Log($"[Currency : <color=pink>{_name}</color>] Withdraw - {withdrawValue}, <color=red> Failed </color>");
                return false;
            }

            _value -= withdrawInCents;
            OnCurrencyUpdate?.Invoke(new CurrencyEvent{ActionType = CurrencyActionType.Withdraw, IsSuccess = true }, _value);
            Debug.Log($"[Currency : <color=pink>{_name}</color>] Withdraw - {withdrawValue}, <color=green> Success </color>");
            return true;
        }

        public bool TrySet(decimal value)
        {
            long setInCents = (long)(value * 100m);
            if (value < 0 || _hasMaxValue && setInCents >= _maxValue)
            {
                OnCurrencyUpdate?.Invoke(new CurrencyEvent
                {
                    ActionType = CurrencyActionType.Set, IsSuccess = false
                }, _value);
                Debug.Log($"[Currency : <color=pink>{_name}</color>] Set value - {value}, <color=red> Failed </color>");
                return false;
            }

            _value = setInCents;
            OnCurrencyUpdate?.Invoke(new CurrencyEvent{ActionType = CurrencyActionType.Set, IsSuccess = true }, _value);
            Debug.Log($"[Currency : <color=pink>{_name}</color>] Set value - {value}, <color=green> Success </color>");
            return true;
        }
        
        #endregion
    }
}
