#if DOTWEEN
using UnityEngine;
using D_Dev.TweenAnimations;

namespace D_dev.Actions
{
    [System.Serializable]
    public class TweenPlayableAction : IAction
    {
        #region Fields

        [SerializeField] private TweenPlayable _tweenPlayable;

        private bool _isFinished;
        private bool _isPlaying;

        #endregion

        #region IAction

        public bool IsFinished => _isFinished;

        public void Execute()
        {
            if (_tweenPlayable == null)
                return;
            
            if (_isPlaying)
                return;

            _tweenPlayable.OnComplete += OnComplete;
            _tweenPlayable.Play();
            _isPlaying = true;
        }

        public void Undo()
        {
            _isFinished = false;
            _isPlaying = false;
        }

        #endregion

        #region Listeners

        private void OnComplete()
        {
            _isFinished = true;
            _isPlaying = false;
            if (_tweenPlayable != null)
                _tweenPlayable.OnComplete -= OnComplete;
        }

        #endregion
    }
}
#endif
