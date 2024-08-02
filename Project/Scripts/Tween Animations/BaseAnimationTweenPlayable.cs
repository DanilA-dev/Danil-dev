#if DOTWEEN
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace TweenAnimations
{
    public abstract class BaseAnimationTweenPlayable : MonoBehaviour
    {
        [SerializeReference] protected BaseAnimationTween[] _tweens;
        [Space]
        [PropertyOrder(-100)]
        [SerializeField] protected bool _playOnEnable;
        [PropertyOrder(-100)]
        [SerializeField] protected bool _playOnStart;
        [PropertySpace(20)]
        [FoldoutGroup("Events")]
        public UnityEvent OnStart;
        [FoldoutGroup("Events")]
        public UnityEvent OnComplete;
      
        protected void OnEnable()
        {
            if(_playOnEnable && HasTweensInArray())
                Play();
        }

        protected void Start()
        {
            if(_playOnStart && HasTweensInArray())
                Play();
        }

        public bool HasTweensInArray() => _tweens != null || _tweens.Length > 0;


        private void Play()
        {
            OnStart?.Invoke();
            OnPlay();
            
            var lastTween = _tweens.Last();
            lastTween.OnComplete.AddListener((() =>
            {
                OnComplete?.Invoke();
                lastTween.OnComplete.RemoveAllListeners();
            }));
        }
        
        public abstract void OnPlay();
        
        public virtual void Pause() {}
        
    }
}
#endif
