﻿#if DOTWEEN
using DG.Tweening;

namespace Project.Scripts.Tween_Animations
{
    public class AnimationTweenPlayer : BaseAnimationTweenPlayable
    {
        public override void OnPlay()
        {
            if(!HasTweensInArray())
                return;
            
            var seq = DOTween.Sequence();

            foreach (var tween in _tweens)
                seq.Join(tween.Play());
            

            seq.SetAutoKill(gameObject);
        }

        public override void Pause()
        {
            if (!HasTweensInArray())
                return;

            foreach (var tween in _tweens)
                tween.Pause();
        }
    }
}
#endif


