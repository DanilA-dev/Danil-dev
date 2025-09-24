#if DOTWEEN
using System.Linq;
using D_Dev.UtilScripts.Tween_Animations.Types;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.UtilScripts.Tween_Animations
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

        #region Properties

        public bool IsComplete {get; protected set;}

        #endregion
        
        #region Monobehaviour

        protected virtual void OnEnable()
        {
            if(_playOnEnable && HasTweensInArray())
                Play();
        }

        protected virtual void Start()
        {
            if(_playOnStart && HasTweensInArray())
                Play();
        }

        #endregion

        #region Public

        public void Play()
        {
            OnStart?.Invoke();
            OnPlay();
            IsComplete = false;

            var lastTween = _tweens.Last();
            lastTween.OnComplete.AddListener((() =>
            {
                OnComplete?.Invoke();
                IsComplete = true;
                lastTween.OnComplete.RemoveAllListeners();
            }));
        }

        #endregion
        
        #region Private

        protected bool HasTweensInArray() => _tweens != null || _tweens.Length > 0;

        #endregion

        #region Virtual/Abstract

        protected abstract void OnPlay();
        
        public virtual void Pause() {}

        #endregion
    }
}
#endif
