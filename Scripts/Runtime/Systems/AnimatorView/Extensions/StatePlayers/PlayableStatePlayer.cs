using D_Dev.AnimatorView.AnimationPlayableHandler;

namespace D_Dev.AnimatorView.Extensions
{
    public class PlayableStatePlayer : AnimationStatePlayer<AnimationPlayableClipConfig, AnimationClipPlayableMixer>
    {
        protected override void OnPlay(AnimationPlayableClipConfig config)
        {
            _player.Play(config);
        }
    }
}
