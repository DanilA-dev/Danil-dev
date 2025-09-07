using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.Scripts.Runtime.UtilScripts.CurrencySystem
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
        [SerializeField, ReadOnly] private int _value;
        [SerializeField] private int _defaultValue;
        [SerializeField] private bool _hasMaxValue;
        [ShowIf(nameof(_hasMaxValue))]
        [SerializeField] private int _maxValue;

        public event Action<CurrencyEvent,int> OnCurrencyUpdate;
            
        #endregion

        #region Properties
        public int Value => _value;

        public bool HasMaxValue
        {
            get => _hasMaxValue;
            set => _hasMaxValue = value;
        }
   

        public int MaxValue
        {
            get => _maxValue;
            set => _maxValue = value;
        }

        public int DefaultValue
        {
            get => _defaultValue;
            set => _defaultValue = value;
        }

        #endregion

        #region Constructors

        public Currency(string name, int initialValue)
        {
            _name = name;
            _value = initialValue;
        }

        #endregion
        
        #region Public

        public bool TryDeposit(int depositValue)
        {
            if (depositValue <= 0 || _hasMaxValue && _value >= MaxValue)
            {
                OnCurrencyUpdate?.Invoke(new CurrencyEvent
                {
                    ActionType = CurrencyActionType.Deposit,
                    IsSuccess = false,
                }, _value);
                Debug.Log($"[Currency : <color=pink>{_name}</color>] Deposit - {depositValue}, <color=red> Failed </color>");
                return false;
            }
            
            var newValue = _value + depositValue;
            if (_hasMaxValue && newValue >= MaxValue)
                _value = MaxValue;
            else
                _value += depositValue;
            
            OnCurrencyUpdate?.Invoke(new CurrencyEvent{ActionType = CurrencyActionType.Deposit, IsSuccess = true }, _value);
            Debug.Log($"[Currency : <color=pink>{_name}</color>] Deposit - {depositValue}, <color=green> Success </color>");
            return true;
        }

        public bool TryWithdraw(int withdrawValue)
        {
            if (withdrawValue <= 0 || _value < withdrawValue)
            {
                OnCurrencyUpdate?.Invoke(new CurrencyEvent
                {
                    ActionType = CurrencyActionType.Withdraw, IsSuccess = false
                }, _value);
                Debug.Log($"[Currency : <color=pink>{_name}</color>] Withdraw - {withdrawValue}, <color=red> Failed </color>");
                return false;
            }
            
            _value -= withdrawValue;
            OnCurrencyUpdate?.Invoke(new CurrencyEvent{ActionType = CurrencyActionType.Withdraw, IsSuccess = true }, _value);
            Debug.Log($"[Currency : <color=pink>{_name}</color>] Withdraw - {withdrawValue}, <color=green> Success </color>");
            return true;
        }

        public bool TrySet(int value)
        {
            if (value <= 0 || _hasMaxValue && value >= MaxValue)
            {
                OnCurrencyUpdate?.Invoke(new CurrencyEvent
                {
                    ActionType = CurrencyActionType.Set, IsSuccess = false
                }, _value);
                Debug.Log($"[Currency : <color=pink>{_name}</color>] Set value - {value}, <color=red> Failed </color>");
                return false;
            }
            
            _value = value;
            OnCurrencyUpdate?.Invoke(new CurrencyEvent{ActionType = CurrencyActionType.Set, IsSuccess = true }, _value);
            Debug.Log($"[Currency : <color=pink>{_name}</color>] Set value - {value}, <color=green> Success </color>");
            return true;
        }
        
        #endregion
    }
}