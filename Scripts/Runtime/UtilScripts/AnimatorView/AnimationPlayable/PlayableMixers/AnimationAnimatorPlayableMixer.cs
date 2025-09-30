using UnityEngine.Animations;
using UnityEngine.Playables;

namespace D_Dev.AnimatorView.AnimationPlayableHandler
{
    public class AnimationAnimatorPlayableMixer : BaseAnimationPlayableMixer
    {
        #region Fields

        private AnimatorControllerPlayable _animatorPlayable;

        #endregion

        #region Properties

        public AnimatorControllerPlayable AnimatorPlayable => _animatorPlayable;

        #endregion

        #region Monobehaviour

        protected override void OnEnable()
        {
            base.OnEnable();
            _animatorPlayable = AnimatorControllerPlayable.Create(_playableGraph.PlayableGraph,
                _playableGraph.Animator.runtimeAnimatorController);
            _playableGraph.RootLayerMixer.ConnectInput(_layer, _animatorPlayable, 0, 1);

            _playableGraph.Animator.runtimeAnimatorController = null;
        }

        #endregion
    }
}
