#if DOTWEEN
using DG.Tweening;

namespace TweenAnimations
{
    [System.Serializable]
    public class DelayTween : BaseAnimationTween
    {
        private Sequence _sequence;
        
        public override Tween Play()
        {
            _sequence = _sequence ?? DOTween.Sequence();
            Tween = _sequence;
            _sequence.AppendInterval(_duration);
            return _sequence;
        }
    }
}
#endif


