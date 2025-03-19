using UnityEngine;

namespace Danil_dev.Scripts.Runtime.UtilScripts.AnimatorView
{
    [System.Serializable]
    public class AnimationConfig
    {
        [field: SerializeField] public AnimationClip AnimationClip { get; set; }
        [field: SerializeField] public int Layer { get; set; }
        [field: SerializeField] public float CrossFadeTime { get; set; }
        [field: SerializeField] public bool Loop { get; set; }
        [field: SerializeField, Range(0, 10)] public float AnimationTime { get; set; } = 1;
    }
}