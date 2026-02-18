using Cysharp.Threading.Tasks;
using D_Dev.CustomEventManager;
using D_Dev.EntitySpawner;
using D_Dev.PolymorphicValueSystem;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using PoolableObject = D_Dev.EntitySpawner.PoolableObject;

namespace D_Dev.CurrencySystem.Extensions
{
    public class CurrencyFlyingAnimationHandler : MonoBehaviour
    {
        #region Classes

        [System.Serializable]
        public class FlyingCurrencyAnimation
        {
            #region Fields

            [FoldoutGroup("General")] 
            [SerializeField] private EntitySpawnSettings _entitySpawnSettings;
            [FoldoutGroup("General")] 
            [SerializeField] private int _maxAmount;
            [FoldoutGroup("General")] 
            [SerializeField] private Vector2 _localScale = new(1, 1);

            [FoldoutGroup("UI Settings")]
            [SerializeField] private Vector2 _fromOffset;
            [FoldoutGroup("UI Settings")]
            [SerializeField] private Vector2 _toOffset;
            [FoldoutGroup("UI Settings")]
            [SerializeReference] private PolymorphicValue<Canvas> _canvas;

            [FoldoutGroup("World Settings")]
            [SerializeField] private Vector3 _fromOffsetWorld;
            [FoldoutGroup("World Settings")]
            [SerializeField] private Vector3 _toOffsetWorld;
            [FoldoutGroup("World Settings")]
            [SerializeField] private float _jumpPower = 2f;
            [FoldoutGroup("World Settings")]
            [SerializeField] private int _numJumps = 1;

            [FoldoutGroup("Animation Settings")]
            [SerializeField] private float _delayBetweenSpawn;
            [FoldoutGroup("Animation Settings")]
            [SerializeField] private float _moveTime;
            [FoldoutGroup("Animation Settings")]
            [SerializeField] private Ease _ease;
            [FoldoutGroup("Animation Settings")]
            [SerializeField] private bool _ignoreTimeScale;

            [FoldoutGroup("Events")] 
            public UnityEvent OnSingleAnimationEnd;
            [FoldoutGroup("Events")] 
            public UnityEvent OnAllAnimationEnd;

            #endregion

            #region Properties

            public EntitySpawnSettings EntitySpawnSettings => _entitySpawnSettings;

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

            public Vector3 FromOffsetWorld => _fromOffsetWorld;

            public Vector3 ToOffsetWorld => _toOffsetWorld;

            public float DelayBetweenSpawn => _delayBetweenSpawn;

            public float JumpPower => _jumpPower;

            public int NumJumps => _numJumps;

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

        private async void Start()
        {
            await _flyingCurrencyAnimation.EntitySpawnSettings.Init();
        }

        private void OnEnable()
        {
            EventManager.AddListener<Transform, Transform, long>(_flyingAnimationEventName.Value, OnCurrencyUpdate);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<Transform, Transform, long>(_flyingAnimationEventName.Value, OnCurrencyUpdate);
        }

        private void OnDestroy()
        {
            _flyingCurrencyAnimation?.EntitySpawnSettings?.DisposePool();
        }


        #endregion

        #region Listeners

        private void OnCurrencyUpdate(Transform from, Transform to, long amount)
        {
            StartFlyingCurrencySequence(from, to, amount).Forget();
        }

        #endregion

        #region Private

        private async UniTaskVoid StartFlyingCurrencySequence(Transform from, Transform to, long amount)
        {
            int createdAmount = Mathf.Min((int)amount, _flyingCurrencyAnimation.MaxAmount);
            bool fromIsUI = from is RectTransform;
            bool toIsUI = to is RectTransform;
            bool isWorldMode = !fromIsUI && !toIsUI;

            for (int i = 0; i < createdAmount; i++)
            {
                var currencyGo = await _flyingCurrencyAnimation.EntitySpawnSettings.Get();
                if (currencyGo == null)
                    continue;

                if (isWorldMode)
                {
                    Transform currencyItem = currencyGo.transform;
                    currencyItem.position = from.position + (Vector3)_flyingCurrencyAnimation.FromOffsetWorld;
                    currencyItem.localScale = _flyingCurrencyAnimation.LocalScale;

                    currencyItem.DOJump(
                        to.position + (Vector3)_flyingCurrencyAnimation.ToOffsetWorld, 
                        _flyingCurrencyAnimation.JumpPower, 
                        _flyingCurrencyAnimation.NumJumps, 
                        _flyingCurrencyAnimation.MoveTime)
                        .SetEase(_flyingCurrencyAnimation.Ease)
                        .SetUpdate(_flyingCurrencyAnimation.IgnoreTimeScale)
                        .OnComplete(() =>
                        {
                            _flyingCurrencyAnimation.OnSingleAnimationEnd?.Invoke();
                            if (currencyGo.TryGetComponent(out PoolableObject poolable))
                            {
                                poolable?.Release();
                            }
                        });
                }
                else
                {
                    var canvasRect = _flyingCurrencyAnimation.Canvas.Value.GetComponent<RectTransform>();

                    Vector2 fromScreenPos = fromIsUI 
                        ? ((RectTransform)from).position 
                        : _camera.WorldToScreenPoint(from.position);

                    Vector2 toScreenPos = toIsUI 
                        ? ((RectTransform)to).position 
                        : _camera.WorldToScreenPoint(to.position);

                    if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, fromScreenPos, null, out Vector2 fromLocalPos))
                        continue;

                    if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, toScreenPos, null, out Vector2 toLocalPos))
                        continue;

                    RectTransform currencyItem = currencyGo.GetComponent<RectTransform>();
                    currencyItem.SetParent(canvasRect, false);
                    currencyItem.anchoredPosition = fromLocalPos + _flyingCurrencyAnimation.FromOffset;
                    currencyItem.localScale = _flyingCurrencyAnimation.LocalScale;

                    currencyItem.DOAnchorPos(toLocalPos + _flyingCurrencyAnimation.ToOffset, _flyingCurrencyAnimation.MoveTime)
                        .SetEase(_flyingCurrencyAnimation.Ease)
                        .SetUpdate(_flyingCurrencyAnimation.IgnoreTimeScale)
                        .OnComplete(() =>
                        {
                            _flyingCurrencyAnimation.OnSingleAnimationEnd?.Invoke();
                            if (currencyGo.TryGetComponent(out PoolableObject poolable))
                            {
                                poolable?.Release();
                            }
                        });
                }

                await UniTask.Delay((int)(_flyingCurrencyAnimation.DelayBetweenSpawn * 1000), DelayType.Realtime);
            }
            _flyingCurrencyAnimation.OnAllAnimationEnd?.Invoke();
        }

        #endregion
    }
}