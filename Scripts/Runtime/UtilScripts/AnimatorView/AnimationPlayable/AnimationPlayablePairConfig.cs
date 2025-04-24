using UnityEngine.Animations;

namespace D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler
{
    public struct AnimationPlayablePairConfig
    {
        public AnimationClipPlayable Playable { get; set; }
        public AnimationPlayableClipConfig ClipConfig { get; set; }
    }
}