namespace D_Dev.AnimatorView.Extensions
{
    public class AnimatorViewStatePlayer : AnimationStatePlayer<AnimationClipConfig, AnimatorView>
    {
        protected override void OnPlay(AnimationClipConfig config)
        {
            _player.PlayAnimation(config);
        }
    }
}
