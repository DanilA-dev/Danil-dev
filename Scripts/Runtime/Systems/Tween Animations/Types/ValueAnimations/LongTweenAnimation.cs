#if DOTWEEN
using DG.Tweening;

namespace D_Dev.TweenAnimations
{
    [System.Serializable]
    public class LongTweenAnimation : BaseTweenValueAnimation<long>
    {
        #region Constructors

        public LongTweenAnimation() {}

        public LongTweenAnimation(long startValue, long endValue, float duration, Ease ease = Ease.Linear)
            : base(startValue, endValue, duration, ease)
        {
        }

        #endregion
        
        #region Overrides
        public override Tween Play()
        {
            if (_text != null)
                SetTarget(_text.gameObject);
            return Tween = DOTween.To(() => _startValue, x => ApplyValue(x), _endValue, Duration).SetEase(_ease);
        }
        #endregion
    }
}
#endif
