using DG.Tweening;
using UnityEngine;

namespace TweenAnimations
{
    [System.Serializable]
    public class RotateAnimationTween : BaseAnimationTween
    {
        [SerializeField] private Transform _rotateObject;
        [SerializeField] private Vector3 _euler;
        
        public override Tween Play()
        {
            Tween = _rotateObject.DORotate(_euler, _duration);
            return Tween;
        }
    }
}