#if DOTWEEN
using Sirenix.OdinInspector;
using UnityEngine;

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
        
        
        public abstract void Play();
        public virtual void Pause() {}
        
    }
}
#endif
