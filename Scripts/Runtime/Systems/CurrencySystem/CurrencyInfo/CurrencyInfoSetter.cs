using System.Collections;
using D_Dev.PolymorphicValueSystem;
using D_Dev.PositionRotationConfig;
using D_Dev.TweenAnimations.Types;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.CurrencySystem
{
    public class CurrencyInfoSetter : MonoBehaviour
    {
        #region Classes

        [System.Serializable]
        public class FlyingCurrencyAnimation
        {
            public GameObject CurrencyItemPrefab;
            [Space]
            public long MaxCurrencyToSpawn;

            [Space]
            [SerializeReference] public BasePositionSettings DestinationPosition;
            [SerializeReference] public BasePositionSettings SpawnPosition;
            [SerializeReference] public PolymorphicValue<Canvas> Canvas = new CanvasConstantValue();
            public Vector2 SpawnOffset;
            public Vector2 DestinationOffset;
            [Space]
            public float DelayBetweenSpawns;
            public MoveAnimationTween AnimationTween;
            [Space]
            public UnityEvent OnAllAnimationsComplete;
        }

        #endregion
        
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
        [SerializeField] private long _amount;
        [SerializeField] private bool _setOnEnable;
        [ShowIf("_setOnEnable")]
        [SerializeField] private StartCurrencySetAction _startCurrencySetAction;
        [FoldoutGroup("Flying Animation")] 
        [SerializeField] private bool _animateFlyingCurrency;
        [ShowIf(nameof(_animateFlyingCurrency))]
        [SerializeField] private FlyingCurrencyAnimation _flyingCurrencyAnimation;
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
            if (_currencyInfo != null && _currencyInfo.Value.Currency.TryDeposit(amount))
            {
                OnDepositSuccess?.Invoke();
                TryAnimateFlyingCurrency(amount);
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
                TryAnimateFlyingCurrency(amount);
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
        
        private void TryAnimateFlyingCurrency(long amount)
        {
            if(!_animateFlyingCurrency)
                return;

            StartCoroutine(FlyingCurrencyCoroutine(amount));
        }

        private IEnumerator FlyingCurrencyCoroutine(long amount)
        {
            var config = _flyingCurrencyAnimation;
            
            if(config.CurrencyItemPrefab == null)
            {
                Debug.LogError("[CurrencyInfoSetter] CurrencyItemPrefab is null");
                yield break;
            }

            if(config.CurrencyItemPrefab.GetComponent<RectTransform>() == null)
            {
                Debug.LogError("[CurrencyInfoSetter] CurrencyItemPrefab must have RectTransform component");
                yield break;
            }
            
            Canvas canvas = config.Canvas?.Value;
            if(canvas == null)
            {
                Debug.LogError("[CurrencyInfoSetter] Canvas is null");
                yield break;
            }

            Camera mainCamera = Camera.main;
            if(mainCamera == null)
            {
                Debug.LogError("[CurrencyInfoSetter] Camera.main is null");
                yield break;
            }

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            if(canvasRect == null)
            {
                Debug.LogError("[CurrencyInfoSetter] Canvas has no RectTransform");
                yield break;
            }

            Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            int spawnCount = (int)Mathf.Min(amount, config.MaxCurrencyToSpawn);
            int completedAnimations = 0;
            int activeAnimations = 0;

            for(int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnWorldPos = config.SpawnPosition?.GetPosition() ?? Vector3.zero;
                Vector3 destWorldPos = config.DestinationPosition?.GetPosition() ?? Vector3.zero;

                spawnWorldPos += new Vector3(config.SpawnOffset.x, config.SpawnOffset.y, 0);
                destWorldPos += new Vector3(config.DestinationOffset.x, config.DestinationOffset.y, 0);

                Vector2 spawnScreenPos = mainCamera.WorldToScreenPoint(spawnWorldPos);
                Vector2 destScreenPos = mainCamera.WorldToScreenPoint(destWorldPos);

                if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect, spawnScreenPos, uiCamera, out Vector2 spawnLocalPos))
                {
                    spawnLocalPos = Vector2.zero;
                }

                if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect, destScreenPos, uiCamera, out Vector2 destLocalPos))
                {
                    destLocalPos = Vector2.zero;
                }

                GameObject currencyObj = Instantiate(config.CurrencyItemPrefab, canvas.transform);
                currencyObj.transform.SetAsLastSibling();

                RectTransform currencyRect = currencyObj.GetComponent<RectTransform>();
                currencyRect.anchoredPosition = spawnLocalPos;

                if(config.AnimationTween != null)
                {
                    activeAnimations++;
                    MoveAnimationTween tween = config.AnimationTween;
                    
                    if(currencyObj.TryGetComponent<Transform>(out Transform currencyTransform))
                    {
                        tween.MovedObject = currencyTransform;
                    }

                    if(tween.MoveType == MoveAnimationTween.MoveObjectType.Vector)
                    {
                        tween.PositionStart = spawnLocalPos;
                        tween.PositionEnd = destLocalPos;
                        tween.UseInitialPositionAsStart = false;
                    }
                    else if(tween.MoveType == MoveAnimationTween.MoveObjectType.Transform)
                    {
                        tween.UseInitialPositionAsStart = true;
                    }

                    var tweener = tween.Play();
                    if(tweener != null)
                    {
                        tweener.OnComplete(() =>
                        {
                            Destroy(currencyObj);
                            completedAnimations++;
                            
                            if(completedAnimations >= activeAnimations && config.OnAllAnimationsComplete != null)
                            {
                                config.OnAllAnimationsComplete.Invoke();
                            }
                        });
                    }
                }
                else
                {
                    Destroy(currencyObj);
                }

                if(config.DelayBetweenSpawns > 0 && i < spawnCount - 1)
                {
                    yield return new WaitForSeconds(config.DelayBetweenSpawns);
                }
            }
        }

        #endregion
        
    }
}
