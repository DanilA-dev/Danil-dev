#if DOTWEEN
using System.Linq;
using Project.Scripts.Tween_Animations.Types;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Scripts.Tween_Animations
{
    public abstract class BaseAnimationTweenPlayable : MonoBehaviour
    {
        #region Fields

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

        #endregion

        #region Monobehaviour

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

        #endregion

        #region Private

        protected bool HasTweensInArray() => _tweens != null || _tweens.Length > 0;
        
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


        #endregion

        #region Virtual/Abstract

        protected abstract void OnPlay();
        
        public virtual void Pause() {}

        #endregion
    }
}
#endif
