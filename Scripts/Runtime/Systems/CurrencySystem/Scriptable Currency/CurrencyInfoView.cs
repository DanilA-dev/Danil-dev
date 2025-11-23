using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.CurrencySystem
{
    public class CurrencyInfoView : MonoBehaviour
    {
        #region Classes

        [System.Serializable]
        public class CurrencyDisplayAnimationSettings
        {
            public float AnimationDuration = 1f;
            public AnimationCurve AnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }

        #endregion
        
        #region Fields

        [SerializeField] private CurrencyInfo _currencyInfo;
        [SerializeField] private string _format = "n0";
        [SerializeField] private string _cultureInfo = "en-US";

        [FoldoutGroup("Animation")] 
        [SerializeField] private bool _useAnimation;
        [ShowIf("_useAnimation")]
        [SerializeField] private CurrencyDisplayAnimationSettings _animationSettings;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent<string> _onCurrencyUpdate;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent _onDepositSuccess;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent _onDepositFailed;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent _onWithdrawSuccess;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent _onWithdrawFailed;
        
        private int _currentDisplayValue;
        private Coroutine _animationCoroutine;

        #endregion

        #region Properties
        
        public UnityEvent<string> OnCurrencyUpdate => _onCurrencyUpdate;
        public UnityEvent OnDepositSuccess => _onDepositSuccess;
        public UnityEvent OnDepositFailed => _onDepositFailed;
        public UnityEvent OnWithdrawSuccess => _onWithdrawSuccess;
        public UnityEvent OnWithdrawFailed => _onWithdrawFailed;
        
        #endregion
        
        #region Monobehaviour

        private void OnEnable()
        {
            if(_currencyInfo != null)
                _currencyInfo.Currency.OnCurrencyUpdate += UpdateCurrency;
        }

        private void Start()
        {
            if(_currencyInfo != null)
            {
                _currentDisplayValue = _currencyInfo.Currency.Value;
                InvokeCurrencyUpdateEvent(_currencyInfo.Currency.Value);
            }
        }

        private void OnDisable()
        {
            if(_currencyInfo != null)
                _currencyInfo.Currency.OnCurrencyUpdate -= UpdateCurrency;
        }

        #endregion

        #region Public

        public void SetCurrencyInfo(CurrencyInfo currencyInfo)
        {
            if (currencyInfo == null)
            {
                Debug.LogError($"[CurrencyInfoView] Trying to set new currencyInfo, but its null!");
                return;
            }
            
            if(_currencyInfo != null && _currencyInfo == currencyInfo)
                return;

            if (_currencyInfo != null)
                _currencyInfo.Currency.OnCurrencyUpdate -= UpdateCurrency;
            
            _currencyInfo = currencyInfo;
            _currencyInfo.Currency.OnCurrencyUpdate += UpdateCurrency;
            InvokeCurrencyUpdateEvent(_currencyInfo.Currency.Value);
        }

        public void DisplayCurrencyValue(CurrencyInfo currencyInfo)
        {
            if(currencyInfo == null)
                return;
            
            if (!_useAnimation)
            {
                InvokeCurrencyUpdateEvent(currencyInfo.Currency.Value);
                _currentDisplayValue = currencyInfo.Currency.Value;
            }
            else
            {
                if (_animationCoroutine != null)
                    StopCoroutine(_animationCoroutine);
                
                _animationCoroutine = StartCoroutine(AnimateCurrencyCoroutine(currencyInfo.Currency.Value));
            }
        }
        
        public void DisplayCurrencyValue(int value)
        {
            if (!_useAnimation)
            {
                InvokeCurrencyUpdateEvent(value);
                _currentDisplayValue = value;
            }
            else
            {
                if (_animationCoroutine != null)
                    StopCoroutine(_animationCoroutine);
                
                _animationCoroutine = StartCoroutine(AnimateCurrencyCoroutine(value));
            }
        }
        
        #endregion

        #region Listeners

        private void UpdateCurrency(Currency.CurrencyEvent currencyEvent, int currencyValue)
        {
            if (!_useAnimation)
            {
                InvokeCurrencyUpdateEvent(currencyValue);
                _currentDisplayValue = currencyValue;
            }
            else
            {
                if (_animationCoroutine != null)
                    StopCoroutine(_animationCoroutine);
                
                _animationCoroutine = StartCoroutine(AnimateCurrencyCoroutine(currencyValue));
            }
            
            switch (currencyEvent.ActionType)
            {
                case Currency.CurrencyActionType.Deposit:
                    if (currencyEvent.IsSuccess)
                        _onDepositSuccess?.Invoke();
                    else
                        _onDepositFailed?.Invoke();
                    break;
                    
                case Currency.CurrencyActionType.Withdraw:
                    if (currencyEvent.IsSuccess)
                        _onWithdrawSuccess?.Invoke();
                    else
                        _onWithdrawFailed?.Invoke();
                    break;
            }
        }

        #endregion

        #region Private
        private void InvokeCurrencyUpdateEvent(int currencyValue)
        {
            _onCurrencyUpdate?.Invoke(currencyValue.ToString(_format, System.Globalization.CultureInfo.GetCultureInfo(_cultureInfo)));
        }
        
        private IEnumerator AnimateCurrencyCoroutine(int targetValue, float? customDuration = null, AnimationCurve customCurve = null)
        {
            int startValue = _currentDisplayValue;
            float duration = customDuration ?? _animationSettings.AnimationDuration;
            AnimationCurve curve = customCurve ?? _animationSettings.AnimationCurve;
            
            float elapsedTime = 0f;
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / duration;
                
                float curveValue = curve.Evaluate(progress);
                
                int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, curveValue));
                
                _currentDisplayValue = currentValue;
                InvokeCurrencyUpdateEvent(currentValue);
                
                yield return null;
            }
            
            _currentDisplayValue = targetValue;
            InvokeCurrencyUpdateEvent(targetValue);
            _animationCoroutine = null;
        }
        
        #endregion
    }
}
