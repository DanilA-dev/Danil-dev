#if DOTWEEN
using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

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
        [ShowIf(nameof(_moveType), MoveType.Transform)]
        [SerializeField] private Transform _moveStart;
        [ShowIf(nameof(_moveType), MoveType.Transform)]
        [SerializeField] private Transform _moveEnd;
        [ShowIf(nameof(_moveType), MoveType.Vector)]
        [SerializeField] private Vector3 _positionStart;
        [HideIf(nameof(_moveType), MoveType.Transform)]
        [SerializeField] private Vector3 _positionEnd;
        
        public override Tween Play()
        {
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
            return _movedObject.DOMove(_moveEnd.position, _duration)
                .From(_moveStart.position);
        }

        private Tween YTween()
        {
            return _movedObject.DOLocalMoveY(_positionEnd.y, _duration);
        }
        
        private Tween XTween()
        {
            return _movedObject.DOLocalMoveX(_positionEnd.x, _duration);
        }
        
        private Tween ZTween()
        {
            return _movedObject.DOLocalMoveZ(_positionEnd.z, _duration);
        }
        
        private Tween VectorWorldTween()
        {
            return _movedObject.DOMove(_positionStart, _duration)
                .From(_positionEnd);
        }
        
        public override void Pause()
        {
            Tween.Pause();
        }
    }
}
#endif

