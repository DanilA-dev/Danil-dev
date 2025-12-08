#if DOTWEEN
using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.TweenAnimations.Types
{
    [System.Serializable]
    public class MoveAnimationTween : BaseAnimationTween
    {
        #region Enums

        public enum MoveObjectType
        {
            Vector,
            Y,
            X,
            Z,
            Transform
        }

        public enum MoveMotionType
        {
            None = 0,
            Shake = 1,
            Punch = 2
        }

        #endregion

        #region Fields

        [SerializeField] private MoveObjectType _moveObjectType;
        [SerializeField] private MoveMotionType _moveMotionType;
        [SerializeField] private Transform _movedObject;
        [SerializeField] private bool _useInitialPositionAsStart;
        [ShowIf("@!_useInitialPositionAsStart && this._moveObjectType == MoveObjectType.Transform")]
        [SerializeField] private Transform _moveStart;
        [ShowIf(nameof(_moveObjectType), MoveObjectType.Transform)]
        [SerializeField] private Transform _moveEnd;
        [ShowIf("@!_useInitialPositionAsStart && this._moveObjectType != MoveObjectType.Transform")]
        [SerializeField] private Vector3 _positionStart;
        [ShowIf("@this._moveObjectType != MoveObjectType.Transform")]
        [SerializeField] private Vector3 _positionEnd;
        [ShowIf(nameof(_moveMotionType), MoveMotionType.Shake)]
        [SerializeField] private Vector3 _shakeStrength = Vector3.one;
        [ShowIf(nameof(_moveMotionType), MoveMotionType.Shake)]
        [SerializeField] private int _vibratoShake = 10;
        [ShowIf(nameof(_moveMotionType), MoveMotionType.Shake)]
        [SerializeField] private float _randomnessShake = 90f;
        [ShowIf(nameof(_moveMotionType), MoveMotionType.Shake)]
        [SerializeField] private bool _fadeOutShake = true;
        [ShowIf(nameof(_moveMotionType), MoveMotionType.Punch)]
        [SerializeField] private Vector3 _punch = Vector3.one;
        [ShowIf(nameof(_moveMotionType), MoveMotionType.Punch)]
        [SerializeField] private int _vibratoPunch = 10;
        [ShowIf(nameof(_moveMotionType), MoveMotionType.Punch)]
        [SerializeField] private float _elasticityPunch = 1f;

        private Vector3 _initialStartPos;

        #endregion

        #region Properties

        public MoveObjectType MoveType
        {
            get => _moveObjectType;
            set => _moveObjectType = value;
        }

        public MoveMotionType MotionType
        {
            get => _moveMotionType;
            set => _moveMotionType = value;
        }

        public Transform MovedObject
        {
            get => _movedObject;
            set => _movedObject = value;
        }

        public bool UseInitialPositionAsStart
        {
            get => _useInitialPositionAsStart;
            set => _useInitialPositionAsStart = value;
        }

        public Transform MoveStart
        {
            get => _moveStart;
            set => _moveStart = value;
        }

        public Transform MoveEnd
        {
            get => _moveEnd;
            set => _moveEnd = value;
        }

        public Vector3 PositionStart
        {
            get => _positionStart;
            set => _positionStart = value;
        }

        public Vector3 PositionEnd
        {
            get => _positionEnd;
            set => _positionEnd = value;
        }

        public Vector3 ShakeStrength
        {
            get => _shakeStrength;
            set => _shakeStrength = value;
        }

        public int VibratoShake
        {
            get => _vibratoShake;
            set => _vibratoShake = value;
        }

        public float RandomnessShake
        {
            get => _randomnessShake;
            set => _randomnessShake = value;
        }

        public bool FadeOutShake
        {
            get => _fadeOutShake;
            set => _fadeOutShake = value;
        }

        public Vector3 Punch
        {
            get => _punch;
            set => _punch = value;
        }

        public int VibratoPunch
        {
            get => _vibratoPunch;
            set => _vibratoPunch = value;
        }

        public float ElasticityPunch
        {
            get => _elasticityPunch;
            set => _elasticityPunch = value;
        }

        #endregion

        #region Override

        public override Tween Play()
        {
            switch (_moveMotionType)
            {
                case MoveMotionType.None:
                    Tween = PlayMoveTween();
                    break;
                case MoveMotionType.Shake:
                    Tween = _movedObject.DOShakePosition(Duration, _shakeStrength, _vibratoShake, _randomnessShake, _fadeOutShake);
                    break;
                case MoveMotionType.Punch:
                    Tween = _movedObject.DOPunchPosition(_punch, Duration, _vibratoPunch, _elasticityPunch);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Tween;
        }

        #endregion

        #region Private

        private Tween PlayMoveTween()
        {
            RectTransform rect = _movedObject.GetComponent<RectTransform>();
            _initialStartPos = rect ? rect.anchoredPosition : _movedObject.position;
            switch (_moveObjectType)
            {
                case MoveObjectType.Vector:
                    return VectorWorldTween();
                case MoveObjectType.Transform:
                    return TransfromTween();
                case MoveObjectType.X:
                    return XTween();
                case MoveObjectType.Y:
                    return YTween();
                case MoveObjectType.Z:
                    return ZTween();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Tween TransfromTween()
        {
            RectTransform rect = _movedObject.GetComponent<RectTransform>();
            return !rect
                ? _movedObject.DOMove(_moveEnd.position, Duration)
                    .From(!_useInitialPositionAsStart? _moveStart.position : _initialStartPos)
                : rect.DOAnchorPos(_moveEnd.position, Duration)
                    .From(!_useInitialPositionAsStart? _moveStart.position : _initialStartPos);

        }

        private Tween YTween()
        {
            RectTransform rect = _movedObject.GetComponent<RectTransform>();
            return !rect
                ? _movedObject.DOLocalMoveY(_positionEnd.y, Duration)
                    .From(!_useInitialPositionAsStart? _positionStart.y : _initialStartPos.y)
                : rect.DOAnchorPosY(_positionEnd.y, Duration)
                    .From(!_useInitialPositionAsStart? new Vector2(rect.anchoredPosition.x, _positionStart.y) 
                        : new Vector2(rect.anchoredPosition.x, _initialStartPos.y));
        }
        
        private Tween XTween()
        {
            RectTransform rect = _movedObject.GetComponent<RectTransform>();
            return !rect
                ? _movedObject.DOLocalMoveX(_positionEnd.x, Duration)
                    .From(!_useInitialPositionAsStart? _positionStart.x : _initialStartPos.x)
                : rect.DOAnchorPosX(_positionEnd.x, Duration)
                    .From(!_useInitialPositionAsStart? new Vector2(_positionStart.x, rect.anchoredPosition.y) 
                        : new Vector2(_initialStartPos.x, rect.anchoredPosition.y));
        }
        
        private Tween ZTween()
        {
            RectTransform rect = _movedObject.GetComponent<RectTransform>();
            if(rect)
                Debug.LogError($"Trying to move by z axis, by moved object is RectTransform");
            return _movedObject.DOLocalMoveZ(_positionEnd.z, Duration);

        }
        
        private Tween VectorWorldTween()
        {
            RectTransform rect = _movedObject.GetComponent<RectTransform>();
            return !rect? _movedObject.DOMove(_positionEnd, Duration)
                .From(!_useInitialPositionAsStart? _positionStart : _initialStartPos)
                    : rect.DOAnchorPos(_positionEnd, Duration)
                        .From(!_useInitialPositionAsStart? _positionStart : _initialStartPos);
        }

        #endregion

        #region Public

        public override void Pause()
        {
            Tween.Pause();
        }

        #endregion
    }
}
#endif
