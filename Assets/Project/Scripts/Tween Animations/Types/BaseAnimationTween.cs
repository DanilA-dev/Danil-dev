#if DOTWEEN
using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace TweenAnimations
{
    [System.Serializable]
    public abstract class BaseAnimationTween
    {
        [SerializeField] protected float _duration;
        [SerializeField] protected Ease _ease;
        [SerializeField] protected bool _ignoreTimeScale;
        [SerializeField] protected int _loops;
        [ShowIf("@this._loops != 0")]
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
                    .SetLoops(_loops, _loopType)
                    .SetUpdate(_ignoreTimeScale)
                    .SetAutoKill();
                _tween.OnComplete((() => OnComplete?.Invoke()));
            }
        }
        
        public abstract Tween Play();
        public virtual void Pause() {}
        
        [FoldoutGroup("Debug")]
        [Button]
        public void DebugPlay() => Play();
    }
}
#endif

