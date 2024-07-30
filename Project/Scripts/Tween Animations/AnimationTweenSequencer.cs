#if DOTWEEN
using DG.Tweening;

namespace TweenAnimations
{
    public class AnimationTweenSequencer : BaseAnimationTweenPlayable
    {
        public override void Play()
        {
            if(!HasTweensInArray())
                return;
            
            var seq = DOTween.Sequence();
            seq.Restart();
            
            foreach (var tween in _tweens)
                seq.Append(tween.Play());
            
            seq.SetAutoKill(gameObject);

        }
    }
}
#endif
