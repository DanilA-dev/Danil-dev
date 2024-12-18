﻿#if DOTWEEN
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Scripts.Tween_Animations.Types
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

        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }

        public Ease Ease
        {
            get => _ease;
            set => _ease = value;
        }

        public bool IgnoreTimeScale
        {
            get => _ignoreTimeScale;
            set => _ignoreTimeScale = value;
        }

        public int Loops
        {
            get => _loops;
            set => _loops = value;
        }

        public LoopType LoopType
        {
            get => _loopType;
            set => _loopType = value;
        }


        public abstract Tween Play();
        public virtual void Pause() {}
        
        [FoldoutGroup("Debug")]
        [Button]
        public void DebugPlay() => Play();
    }
}
#endif

