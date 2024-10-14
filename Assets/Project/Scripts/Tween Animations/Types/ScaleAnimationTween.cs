#if DOTWEEN
using DG.Tweening;
using UnityEngine;

namespace TweenAnimations
{
    [System.Serializable]
    public class ScaleAnimationTween : BaseAnimationTween
    {
        [SerializeField] private Transform _scaleObject;
        [SerializeField] private Vector3 _endScale;
        [SerializeField] private Vector3 _startScale;
        
        public override Tween Play()
        {
            Tween = _scaleObject.DOScale(_endScale, _duration)
                .From(_startScale);
            
            return Tween;
        }

        public override void Pause()
        {
            Tween.Pause();
        }
    }
}
#endif
