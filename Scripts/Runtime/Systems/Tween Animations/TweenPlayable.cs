#if DOTWEEN
using System;
using System.Collections.Generic;
using DG.Tweening;
using D_Dev.TweenAnimations.Types;
using UnityEngine;

namespace D_Dev.TweenAnimations
{
    [System.Serializable]
    public class TweenPlayable
    {
        #region Enums

        public enum PlayMode
        {
            Parallel = 0,
            Sequential = 1
        }

        #endregion

        #region Fields

        [SerializeField] private PlayMode _playMode;
        [SerializeReference] protected List<BaseAnimationTween> _tweens = new();
        
        public event Action OnStart;
        public event Action OnComplete;
        
        #endregion

        #region Public

        public void Play()
        {
            if (!HasTweensInArray())
                return;

            OnStart?.Invoke();

            var seq = DOTween.Sequence();

            if (_playMode == PlayMode.Parallel)
            {
                foreach (var tween in _tweens)
                    seq.Join(tween.Play());
            }
            else
            {
                foreach (var tween in _tweens)
                    seq.Append(tween.Play());
            }

            seq.SetAutoKill(true);
            var lastTween = _tweens[^1];
            lastTween.OnComplete.AddListener(OnTweensComplete);
        }

        public void Play(int index)
        {
            if (!HasTweensInArray() || index < 0 || index >= _tweens.Count)
                return;

            OnStart?.Invoke();

            _tweens[index].Play();
            _tweens[index].OnComplete.AddListener(OnTweensComplete);
        }

        public void Pause()
        {
            if (!HasTweensInArray())
                return;

            foreach (var tween in _tweens)
                tween.Pause();
        }

        #endregion

        #region Listeners

        private void OnTweensComplete()
        {
            if(!HasTweensInArray())
                return;
                
            OnComplete?.Invoke();
            
            foreach (var tween in _tweens)
                tween.OnComplete.RemoveAllListeners();
        }

        #endregion
        
        #region Private

        private bool HasTweensInArray() => _tweens != null && _tweens.Count > 0;

        #endregion
    }
}
#endif
