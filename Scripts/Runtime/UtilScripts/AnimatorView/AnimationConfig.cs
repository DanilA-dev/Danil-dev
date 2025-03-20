using Sirenix.OdinInspector;
using UnityEngine;

namespace Danil_dev.Scripts.Runtime.UtilScripts.AnimatorView
{
    [System.Serializable]
    public class AnimationConfig
    {
        [field: SerializeField] public bool IsRandomizeClip { get; set; }
        [field: SerializeField, HideIf(nameof(IsRandomizeClip))] public AnimationClip AnimationClip { get; set; }
        [field: SerializeField, ShowIf(nameof(IsRandomizeClip))] public AnimationClip[] AnimationClips { get; set; }
        [field: SerializeField] public int Layer { get; set; }
        [field: SerializeField] public float CrossFadeTime { get; set; }
        [field: SerializeField] public bool Loop { get; set; }
        [field: SerializeField, Range(0, 10)] public float AnimationTime { get; set; } = 1;
        
        public AnimationClip GetAnimationClip() => IsRandomizeClip 
            ? AnimationClips[Random.Range(0, AnimationClips.Length)] 
            : AnimationClip;
    }
}