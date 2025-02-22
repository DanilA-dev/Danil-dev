#if DOTWEEN
using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.UtilScripts.Tween_Animations.Types
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

        #endregion

        #region Fields

        [SerializeField] private MoveObjectType _moveObjectType;
        [SerializeField] private Transform _movedObject;
        [SerializeField] private bool _useInitialPositionAsStart;
        [ShowIf("@!_useInitialPositionAsStart && this._moveType == MoveType.Transform")]
        [SerializeField] private Transform _moveStart;
        [ShowIf(nameof(_moveObjectType), MoveObjectType.Transform)]
        [SerializeField] private Transform _moveEnd;
        [ShowIf("@!_useInitialPositionAsStart && this._moveType != MoveType.Transform")]
        [SerializeField] private Vector3 _positionStart;
        [ShowIf("@this._moveType != MoveType.Transform")]
        [SerializeField] private Vector3 _positionEnd;

        private Vector3 _initialStartPos;

        #endregion

        #region Properties

        public MoveObjectType MoveType
        {
            get => _moveObjectType;
            set => _moveObjectType = value;
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

        #endregion

        #region Override

        public override Tween Play()
        {
            RectTransform rect = _movedObject.GetComponent<RectTransform>();
            _initialStartPos = rect ? rect.anchoredPosition : _movedObject.position;
            switch (_moveObjectType)
            {
                case MoveObjectType.Vector:
                    Tween = VectorWorldTween();
                    break;
                case MoveObjectType.Transform:
                    Tween = TransfromTween();
                    break;
                case MoveObjectType.X:
                    Tween = XTween();
                    break;
                case MoveObjectType.Y:
                    Tween = YTween();
                    break;
                case MoveObjectType.Z:
                    Tween = ZTween();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Tween;
        }

        #endregion

        #region Private

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

