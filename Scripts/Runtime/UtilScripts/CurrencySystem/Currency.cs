using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GlebSherzhukov.Scripts.Game.CurrencySystem
{
    [System.Serializable]
    public class Currency
    {
        #region Enums

        public enum CurrencyActionType
        {
            Deposit = 0,
            Withdraw = 1
        }

        public enum CurrencyMaxValueType
        {
            Auto = 0,
            Manual = 1
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
        [SerializeField, ReadOnly] private int _value;
        [SerializeField] private bool _hasMaxValue;
        [ShowIf(nameof(_hasMaxValue))]
        [SerializeField] private CurrencyMaxValueType _currencyMaxValueType;
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

        public CurrencyMaxValueType MaxValueType
        {
            get => _currencyMaxValueType;
            set => _currencyMaxValueType = value;
        }

        public int MaxValue
        {
            get => _currencyMaxValueType == CurrencyMaxValueType.Manual ? _maxValue : int.MaxValue;
            set
            {
                if(_currencyMaxValueType == CurrencyMaxValueType.Manual)
                    _maxValue = value;
            }
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

        public void Deposit(int depositValue)
        {
            if (depositValue <= 0 || _hasMaxValue && _value >= MaxValue)
            {
                OnCurrencyUpdate?.Invoke(new CurrencyEvent
                {
                    ActionType = CurrencyActionType.Deposit,
                    IsSuccess = false,
                }, _value);
                Debug.Log($"[Currency : <color=pink>{_name}</color>] Deposit - {depositValue}, <color=red> Failed </color>");
                return;
            }
            
            _value += depositValue;
            OnCurrencyUpdate?.Invoke(new CurrencyEvent{ActionType = CurrencyActionType.Deposit, IsSuccess = true }, _value);
            Debug.Log($"[Currency : <color=pink>{_name}</color>] Deposit - {depositValue}, <color=green> Success </color>");
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

        public void Set(int value)
        {
            _value = value;
        }
        
        #endregion
    }
}