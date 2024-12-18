﻿#if DOTWEEN
using DG.Tweening;

namespace Project.Scripts.Tween_Animations.Types
{
    [System.Serializable]
    public class DelayTween : BaseAnimationTween
    {
        private Sequence _sequence;
        
        public override Tween Play()
        {
            _sequence = _sequence ?? DOTween.Sequence();
            Tween = _sequence;
            _sequence.AppendInterval(Duration);
            return _sequence;
        }
    }
}
#endif


