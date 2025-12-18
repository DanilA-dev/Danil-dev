using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.CurrencySystem
{
    public class CurrencyInfoSetter : MonoBehaviour
    {
        #region Enums

        public enum StartCurrencySetAction
        {
            Withdraw,
            Deposit,
            Set
        }

        #endregion
        
        #region Fields

        [SerializeField] private CurrencyInfo _currencyInfo;
        [SerializeField] private long _amount;
        [SerializeField] private bool _setOnEnable;
        [ShowIf("_setOnEnable")]
        [SerializeField] private StartCurrencySetAction _startCurrencySetAction;
        [Space]
        [FoldoutGroup("Deposit Events")]
        public UnityEvent OnDepositSuccess;
        [FoldoutGroup("Deposit Events")]
        public UnityEvent OnDepositFailed;
        [FoldoutGroup("Withdraw Events")]
        public UnityEvent OnWithdrawSuccess;
        [FoldoutGroup("Withdraw Events")]
        public UnityEvent OnWithdrawFailed;
        [FoldoutGroup("Set Events")]
        public UnityEvent OnSetSuccess;
        [FoldoutGroup("Set Events")]
        public UnityEvent OnSetFailed;
            
        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            SetCurrencyOnEnable();
        }


        #endregion
        
        #region Public

        public void TryDeposit() => TryDepositValue(_amount);

        public void TryWithdraw() => TryWithdrawValue(_amount);

        public void TrySet() => TrySetValue(_amount);

        public bool TryDepositValue(long amount)
        {
            if (_currencyInfo != null && _currencyInfo.Currency.TryDeposit(amount))
            {
                OnDepositSuccess?.Invoke();
                return true;
            }
            OnDepositFailed?.Invoke();
            return false;
        }

        public bool TryWithdrawValue(long amount)
        {
            if (_currencyInfo != null && _currencyInfo.Currency.TryWithdraw(amount))
            {
                OnWithdrawSuccess?.Invoke();
                return true;
            }
            OnWithdrawFailed?.Invoke();
            return false;
        }

        public bool TrySetValue(long amount)
        {
            if (_currencyInfo != null && _currencyInfo.Currency.TrySet(amount))
            {
                OnSetSuccess?.Invoke();
                return true;
            }
            OnSetFailed?.Invoke();
            return false;
        }

        #endregion

        #region Private
        private void SetCurrencyOnEnable()
        {
            if (_setOnEnable)
            {
                switch (_startCurrencySetAction)
                {
                    case StartCurrencySetAction.Withdraw:
                        TryWithdraw();
                        break;
                    case StartCurrencySetAction.Deposit:
                        TryDeposit();
                        break;
                    case StartCurrencySetAction.Set:
                        TrySet();
                        break;
                }
            }
        }

        #endregion
        
    }
}
