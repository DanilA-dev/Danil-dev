using D_Dev.CustomEventManager;
using D_Dev.PolymorphicValueSystem;
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

        [SerializeReference] private PolymorphicValue<CurrencyInfo> _currencyInfo;
        [SerializeReference] private PolymorphicValue<int> _amount = new IntConstantValue();
        [SerializeField] private bool _setOnEnable;
        [ShowIf("_setOnEnable")]
        [SerializeField] private StartCurrencySetAction _startCurrencySetAction;
        [SerializeField] private bool _useFlyingAnimation;
        [ShowIf(nameof(_useFlyingAnimation))] 
        [SerializeReference] private PolymorphicValue<string> _flyingAnimationEventName = new StringConstantValue();
        [ShowIf(nameof(_useFlyingAnimation))]
        [SerializeReference] private PolymorphicValue<Transform> _from;
        [ShowIf(nameof(_useFlyingAnimation))]
        [SerializeReference] private PolymorphicValue<Transform> _to;
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

        public void TryDeposit() => TryDepositValue(_amount.Value);

        public void TryWithdraw() => TryWithdrawValue(_amount.Value);

        public void TrySet() => TrySetValue(_amount.Value);

        public bool TryDepositValue(long amount)
        {
            if (_currencyInfo != null && _currencyInfo.Value.Currency.TryDeposit(amount))
            {
                OnDepositSuccess?.Invoke();
                if(_useFlyingAnimation)
                    EventManager.Invoke(_flyingAnimationEventName.Value, _from.Value, _to.Value, amount);
                return true;
            }
            OnDepositFailed?.Invoke();
            return false;
        }

        public bool TryWithdrawValue(long amount)
        {
            if (_currencyInfo != null && _currencyInfo.Value.Currency.TryWithdraw(amount))
            {
                OnWithdrawSuccess?.Invoke();
                if(_useFlyingAnimation)
                    EventManager.Invoke(_flyingAnimationEventName.Value, _from.Value, _to.Value, amount);
                return true;
            }
            OnWithdrawFailed?.Invoke();
            return false;
        }

        

        public bool TrySetValue(long amount)
        {
            if (_currencyInfo != null && _currencyInfo.Value.Currency.TrySet(amount))
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
