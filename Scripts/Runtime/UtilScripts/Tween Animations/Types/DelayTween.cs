#if DOTWEEN
using DG.Tweening;

namespace D_Dev.TweenAnimations.Types
{
    [System.Serializable]
    public class DelayTween : BaseAnimationTween
    {
        #region Fields

        private Sequence _sequence;

        #endregion

        #region Override

        public override Tween Play()
        {
            _sequence = _sequence ?? DOTween.Sequence();
            Tween = _sequence;
            _sequence.AppendInterval(Duration);
            return _sequence;
        }

        #endregion
    }
}
#endif
