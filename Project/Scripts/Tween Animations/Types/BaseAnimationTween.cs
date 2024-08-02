#if DOTWEEN
using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace TweenAnimations
{
    [System.Serializable]
    public abstract class BaseAnimationTween
    {
        [SerializeField] protected float _duration;
        [SerializeField] protected Ease _ease;
        [SerializeField] protected bool _ignoreTimeScale;
        [SerializeField] protected bool _loop;
        [ShowIf(nameof(_loop))]
        [SerializeField] protected LoopType _loopType;

        [PropertySpace(10)] 
        [PropertyOrder(100)] 
        [FoldoutGroup("Events")] 
        public UnityEvent OnStart;
        [PropertyOrder(100)] 
        [FoldoutGroup("Events")]
        public UnityEvent OnComplete;

        private Tween _tween;
        
        protected Tween Tween
        {
            get => _tween;
            set
            {
                _tween = value;
                if (_tween == null)
                    return;
                
                _tween.OnStart((() => OnStart?.Invoke()));
                _tween.SetEase(_ease)
                    .SetLoops(_loop ? Int32.MaxValue : 0, _loopType)
                    .SetUpdate(_ignoreTimeScale)
                    .SetAutoKill();
                _tween.OnComplete((() => OnComplete?.Invoke()));
            }
        }

        public abstract Tween Play();

        public virtual void Pause() {}
    }
}
#endif

