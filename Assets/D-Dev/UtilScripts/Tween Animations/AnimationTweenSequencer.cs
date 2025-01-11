#if DOTWEEN
using DG.Tweening;

namespace D_Dev.UtilScripts.Tween_Animations
{
    public class AnimationTweenSequencer : BaseAnimationTweenPlayable
    {
        #region Override

        protected override void OnPlay()
        {
            if(!HasTweensInArray())
                return;
            
            var seq = DOTween.Sequence();
            seq.Restart();
            
            foreach (var tween in _tweens)
                seq.Append(tween.Play());
            
            seq.SetAutoKill(gameObject);
        }

        #endregion
    }
}
#endif
