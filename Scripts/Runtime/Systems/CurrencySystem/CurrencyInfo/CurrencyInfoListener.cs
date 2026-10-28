using System;
using D_Dev.PolymorphicValueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.CurrencySystem
{
    public class CurrencyInfoListener : MonoBehaviour
    {
        #region Fields

        [SerializeReference] private PolymorphicValue<CurrencyInfo> _currencyInfo;
        
        [FoldoutGroup("Deposit Events")]
        public UnityEvent<long> OnDepositSuccess;
        [FoldoutGroup("Deposit Events")]
        public UnityEvent<long> OnDepositFailed;
        
        [FoldoutGroup("Withdraw Events")]
        public UnityEvent<long> OnWithdrawSuccess;
        [FoldoutGroup("Withdraw Events")]
        public UnityEvent<long> OnWithdrawFailed;
        
        [FoldoutGroup("Set Events")]
        public UnityEvent<long> OnSetSuccess;
        [FoldoutGroup("Set Events")]
        public UnityEvent<long> OnSetFailed;
        
        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            if (_currencyInfo != null && _currencyInfo.Value.Currency != null)
                _currencyInfo.Value.Currency.OnCurrencyUpdate += OnCurrencyUpdate;
        }

        private void OnDisable()
        {
            if (_currencyInfo != null && _currencyInfo.Value.Currency != null)
                _currencyInfo.Value.Currency.OnCurrencyUpdate -= OnCurrencyUpdate;
        }

        #endregion

        #region Listeners

        private void OnCurrencyUpdate(Currency.CurrencyEvent eventData, long value)
        {
            switch (eventData.ActionType)
            {
                case Currency.CurrencyActionType.Deposit:
                    if (eventData.IsSuccess)
                        OnDepositSuccess?.Invoke(value);
                    else
                        OnDepositFailed?.Invoke(value);
                    break;
                case Currency.CurrencyActionType.Withdraw:
                    if (eventData.IsSuccess)
                        OnWithdrawSuccess?.Invoke(value);
                    else
                        OnWithdrawFailed?.Invoke(value);
                    break;
                case Currency.CurrencyActionType.Set:
                    if (eventData.IsSuccess)
                        OnSetSuccess?.Invoke(value);
                    else
                        OnSetFailed?.Invoke(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}
