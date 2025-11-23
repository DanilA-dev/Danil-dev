using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace D_Dev.AnimatorView.AnimationPlayableHandler
{
    public class AnimationSimpleBlendMixer : BaseAnimationPlayableMixer
    {
        #region Classes

        [System.Serializable]
        public class MotionClip
        {
            #region Fields

            [SerializeField] private string _motionName;
            [SerializeField, Range(0,1)] private float _value;
            [SerializeField] private AnimationPlayableClipConfig _config;

            #endregion

            #region Properties

            public float Value
            {
                get => _value;
                set => _value = value;
            }

            public AnimationPlayableClipConfig Config
            {
                get => _config;
                set => _config = value;
            }

            #endregion
        }

        #endregion

        #region Fields

        [PropertySpace(10)]
        [SerializeField, Range(0,1)] private float _value;
        [SerializeField] private bool _debugValue;
        [Space]
        [SerializeField] private MotionClip[] _blendAnimations;
        
        private AnimationLayerMixerPlayable _animationLayerMixer;

        #endregion

        #region Monobehaviour

        protected override void OnEnable()
        {
            base.OnEnable();
            var inputCount = Mathf.Max(1, _blendAnimations.Length);
            _animationLayerMixer = AnimationLayerMixerPlayable.Create(_playableGraph.PlayableGraph,inputCount);
            _playableGraph.RootLayerMixer.ConnectInput(_layer, _animationLayerMixer, 0, 1);
            _playableGraph.RootLayerMixer.SetLayerAdditive((uint)_layer, _isAdditive);
            
            InitBlendClips();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if(_debugValue)
                EvaluateBlendAnimations(_value);
        }
#endif
        #endregion

        #region Private

        private void InitBlendClips()
        {
            if(_blendAnimations.Length <= 0)
                return;

            foreach (var blendAnimation in _blendAnimations)
            {
                var newPlayable = CreatePlayableClip(blendAnimation.Config);
                _animationLayerMixer.ConnectInput(blendAnimation.Config.Layer, newPlayable, 0);
                
                if(blendAnimation.Config.Mask != null)
                    _animationLayerMixer.SetLayerMaskFromAvatarMask((uint)blendAnimation.Config.Layer,blendAnimation.Config.Mask);
            }
            _animationLayerMixer.SetInputWeight(_blendAnimations[0].Config.Layer, 1);
        }

        private void SetWeight(AnimationPlayableClipConfig config, float weight)
        {
            if(!config.IsStatic)
                _animationLayerMixer.SetInputWeight(config.Layer, weight);
        }
        
        #endregion
        
        #region Public

        public void EvaluateBlendAnimations(float value)
        {
            var weight = Mathf.Clamp01(value);
            _value = weight;
            
            var sortedAnimations = _blendAnimations
                .OrderBy(a => a.Value).ToArray();
            
            if (sortedAnimations.Length == 0)
                return;

            MotionClip lower = null, higher = null;

            for (int i = 0; i < sortedAnimations.Length; i++)
            {
                var anim = sortedAnimations[i];
                if (anim.Value <= weight)
                    lower = anim;
                else
                {
                    higher = anim;
                    break; 
                }
            }

            if (lower == null) 
            {
                foreach (var anim in sortedAnimations)
                    SetWeight(anim.Config, anim == sortedAnimations[0] ? 1f : 0f);
                
                return;
            }
            
            if (higher == null)
            {
                foreach (var anim in sortedAnimations)
                    SetWeight(anim.Config, anim == sortedAnimations.Last() ? 1f : 0f);
                
                return;
            }

            float t = (weight - lower.Value) / (higher.Value - lower.Value);
            SetWeight(lower.Config, 1f - t);
            SetWeight(higher.Config, t);

            foreach (var anim in sortedAnimations)
            {
                if (anim != lower && anim != higher)
                    SetWeight(anim.Config, 0f);
            }
        }

        #endregion
    }
}
