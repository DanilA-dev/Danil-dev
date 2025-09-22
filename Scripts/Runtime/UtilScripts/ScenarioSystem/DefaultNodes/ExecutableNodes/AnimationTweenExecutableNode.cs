#if DOTWEEN
using UnityEngine;
using D_Dev.UtilScripts.Tween_Animations;

namespace D_dev.Scripts.ScenarioSystem
{
    public class AnimationTweenExecutableNode : BaseScenarioExecutableNode
    {
        #region Fields

        [SerializeField] private BaseAnimationTweenPlayable _tweenPlayable;

        private bool _isExecuting;

        #endregion

        #region Overrides

        public override void Execute()
        {
            if (_isExecuting || _tweenPlayable == null)
                return;

            _isExecuting = true;

            _tweenPlayable.OnComplete.AddListener(OnSequencerComplete);
            _tweenPlayable.Play();
        }

        #endregion

        #region Monobehaviour

        private void OnDestroy()
        {
            if (_tweenPlayable != null)
                _tweenPlayable.OnComplete.RemoveListener(OnSequencerComplete);
        }

        #endregion

        #region Private

        private void OnSequencerComplete()
        {
            IsFinished = true;
            _isExecuting = false;
        }

        #endregion
    }
}
#endif