#if DOTWEEN
using DG.Tweening;

namespace Project.Scripts.Tween_Animations
{
    public class AnimationTweenSequencer : BaseAnimationTweenPlayable
    {
        public override void OnPlay()
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
