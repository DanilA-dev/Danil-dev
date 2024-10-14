#if DOTWEEN
using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace TweenAnimations
{
    public enum MoveType
    {
        Vector,
        Y,
        X,
        Z,
        Transform
    }
    
    [System.Serializable]
    public class MoveAnimationTween : BaseAnimationTween
    {

        [SerializeField] private MoveType _moveType;
        [SerializeField] private Transform _movedObject;
        [SerializeField] private bool _useInitialPositionAsStart;
        [ShowIf("@!_useInitialPositionAsStart && this._moveType == MoveType.Transform")]
        [SerializeField] private Transform _moveStart;
        [ShowIf(nameof(_moveType), MoveType.Transform)]
        [SerializeField] private Transform _moveEnd;
        [ShowIf("@!_useInitialPositionAsStart && this._moveType != MoveType.Transform")]
        [SerializeField] private Vector3 _positionStart;
        [ShowIf("@this._moveType != MoveType.Transform")]
        [SerializeField] private Vector3 _positionEnd;

        private Vector3 _initialStartPos;
        
        public override Tween Play()
        {
            RectTransform rect = _movedObject.GetComponent<RectTransform>();
            _initialStartPos = rect ? rect.anchoredPosition : _movedObject.position;
            switch (_moveType)
            {
                case MoveType.Vector:
                    Tween = VectorWorldTween();
                    break;
                case MoveType.Transform:
                    Tween = TransfromTween();
                    break;
                case MoveType.X:
                    Tween = XTween();
                    break;
                case MoveType.Y:
                    Tween = YTween();
                    break;
                case MoveType.Z:
                    Tween = ZTween();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Tween;
        }

        private Tween TransfromTween()
        {
            RectTransform rect = _movedObject.GetComponent<RectTransform>();
            return !rect
                ? _movedObject.DOMove(_moveEnd.position, _duration)
                    .From(!_useInitialPositionAsStart? _moveStart.position : _initialStartPos)
                : rect.DOAnchorPos(_moveEnd.position, _duration)
                    .From(!_useInitialPositionAsStart? _moveStart.position : _initialStartPos);

        }

        private Tween YTween()
        {
            RectTransform rect = _movedObject.GetComponent<RectTransform>();
            return !rect
                ? _movedObject.DOLocalMoveY(_positionEnd.y, _duration)
                    .From(!_useInitialPositionAsStart? _positionStart.y : _initialStartPos.y)
                : rect.DOAnchorPosY(_positionEnd.y, _duration)
                    .From(!_useInitialPositionAsStart? new Vector2(rect.anchoredPosition.x, _positionStart.y) 
                        : new Vector2(rect.anchoredPosition.x, _initialStartPos.y));
        }
        
        private Tween XTween()
        {
            RectTransform rect = _movedObject.GetComponent<RectTransform>();
            return !rect
                ? _movedObject.DOLocalMoveX(_positionEnd.x, _duration)
                    .From(!_useInitialPositionAsStart? _positionStart.x : _initialStartPos.x)
                : rect.DOAnchorPosX(_positionEnd.x, _duration)
                    .From(!_useInitialPositionAsStart? new Vector2(_positionStart.x, rect.anchoredPosition.y) 
                        : new Vector2(_initialStartPos.x, rect.anchoredPosition.y));
        }
        
        private Tween ZTween()
        {
            RectTransform rect = _movedObject.GetComponent<RectTransform>();
            if(rect)
                Debug.LogError($"Trying to move by z axis, by moved object is RectTransform");
            return _movedObject.DOLocalMoveZ(_positionEnd.z, _duration);

        }
        
        private Tween VectorWorldTween()
        {
            RectTransform rect = _movedObject.GetComponent<RectTransform>();
            return !rect? _movedObject.DOMove(_positionEnd, _duration)
                .From(!_useInitialPositionAsStart? _positionStart : _initialStartPos)
                    : rect.DOAnchorPos(_positionEnd, _duration)
                        .From(!_useInitialPositionAsStart? _positionStart : _initialStartPos);
        }
        
        public override void Pause()
        {
            Tween.Pause();
        }
    }
}
#endif

