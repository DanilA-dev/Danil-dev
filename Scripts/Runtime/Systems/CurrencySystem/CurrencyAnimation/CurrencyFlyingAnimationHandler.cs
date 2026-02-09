using Cysharp.Threading.Tasks;
using D_Dev.CustomEventManager;
using D_Dev.PolymorphicValueSystem;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.CurrencySystem.Extensions
{
    public class CurrencyFlyingAnimationHandler : MonoBehaviour
    {
        #region Classes

        [System.Serializable]
        public class FlyingCurrencyAnimation
        {
            #region Fields

            [Title("General")]
            [SerializeField] private GameObject _currencyItemPrefab;
            [SerializeField] private int _maxAmount;
            [SerializeField] private Vector2 _localScale = new(1, 1);

            [Title("Offsets")] 
            [SerializeField] private Vector2 _fromOffset;
            [SerializeField] private Vector2 _toOffset;
            
            [Title("Canvas")]
            [SerializeReference] private PolymorphicValue<Canvas> _canvas;

            [Title("Tween Settings")] 
            [SerializeField] private float _delayBetweenSpawn;
            [SerializeField] private float _moveTime;
            [SerializeField] private Ease _ease;
            [SerializeField] private bool _ignoreTimeScale;

            [FoldoutGroup("Events")] 
            public UnityEvent OnSingleAnimationEnd;
            [FoldoutGroup("Events")] 
            public UnityEvent OnAllAnimationEnd;

            #endregion

            #region Properties

            public GameObject CurrencyItemPrefab => _currencyItemPrefab;

            public PolymorphicValue<Canvas> Canvas => _canvas;

            public float MoveTime => _moveTime;

            public Ease Ease => _ease;

            public bool IgnoreTimeScale => _ignoreTimeScale;

            public int MaxAmount => _maxAmount;

            public Vector2 LocalScale
            {
                get => _localScale;
                set => _localScale = value;
            }

            public Vector2 FromOffset => _fromOffset;

            public Vector2 ToOffset => _toOffset;

            public float DelayBetweenSpawn => _delayBetweenSpawn;

            #endregion
        }

        #endregion
        
        #region Fields

        [Title("Currency")] 
        [SerializeReference] private PolymorphicValue<string> _flyingAnimationEventName = new StringConstantValue();

        [Title("Flying Animations")] 
        [SerializeField] private FlyingCurrencyAnimation _flyingCurrencyAnimation;

        private Camera _camera;
        
        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            EventManager.AddListener<Transform, Transform, long>(_flyingAnimationEventName.Value, OnCurrencyUpdate);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<Transform, Transform, long>(_flyingAnimationEventName.Value, OnCurrencyUpdate);
        }


        #endregion

        #region Listeners

        private void OnCurrencyUpdate(Transform from, Transform to, long amount)
        {
            StartFlyingCurrencySequence(from, to, amount);
        }

        #endregion

        #region Private

        private async void StartFlyingCurrencySequence(Transform from, Transform to, long amount)
        {
            int createdAmount = Mathf.Min((int)amount, _flyingCurrencyAnimation.MaxAmount);
            var canvasRect = _flyingCurrencyAnimation.Canvas.Value.GetComponent<RectTransform>();

            Vector3 worldPos = from.position;
            Vector2 screenPos = _camera.WorldToScreenPoint(worldPos);
            
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    screenPos,
                    null,
                    out Vector2 fromLocalPos))
            {
                for (int i = 0; i < createdAmount; i++)
                {
                    RectTransform currencyItem = Instantiate(
                        _flyingCurrencyAnimation.CurrencyItemPrefab.GetComponent<RectTransform>(),
                        canvasRect,
                        false);

                    var fromAnchoredPos = fromLocalPos + _flyingCurrencyAnimation.FromOffset;
                    currencyItem.anchoredPosition = fromAnchoredPos;
                    currencyItem.localScale = _flyingCurrencyAnimation.LocalScale;

                    currencyItem.DOMove(to.position, _flyingCurrencyAnimation.MoveTime)
                        .SetEase(_flyingCurrencyAnimation.Ease)
                        .SetUpdate(_flyingCurrencyAnimation.IgnoreTimeScale)
                        .OnComplete(() =>
                        {
                            _flyingCurrencyAnimation.OnSingleAnimationEnd?.Invoke();
                            Destroy(currencyItem.gameObject);
                        })
                        .SetAutoKill();

                    await UniTask.Delay((int)_flyingCurrencyAnimation.DelayBetweenSpawn * 1000, DelayType.Realtime);
                }
                _flyingCurrencyAnimation.OnAllAnimationEnd?.Invoke();
            }
        }

        #endregion
    }
}