using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace D_Dev.AnimatorView.AnimationPlayableHandler
{
    public class AnimationClipPlayableMixer : BaseAnimationPlayableMixer
    {
        #region Fields

        [PropertyOrder(-1)]
        [SerializeField] private bool _connectToSeperateMixer;
        [PropertySpace(10)] 
        [SerializeField] private bool _hasStartAnimations;
        [ShowIf(nameof(_hasStartAnimations))]
        [SerializeField] private AnimationPlayableClipConfig[] _onStartAnimations;
        
        private AnimationPlayableClipConfig _lastPlayedConfig;
        private AnimationLayerMixerPlayable _targetLayerMixerPlayable;
        
        private Dictionary<int, AnimationPlayablePairConfig> _playablesPair = new();

        private Coroutine _blendInCoroutine;
        private Coroutine _blendOutCoroutine;
        
        #endregion

        #region Monobehaviour

        protected override void OnEnable()
        {
            base.OnEnable();

            _targetLayerMixerPlayable = _connectToSeperateMixer 
                ? AnimationLayerMixerPlayable.Create(_playableGraph.PlayableGraph, 1) 
                : _playableGraph.RootLayerMixer;
            
            if(_connectToSeperateMixer && _targetLayerMixerPlayable.IsValid())
                _playableGraph.RootLayerMixer.ConnectInput(_layer, _targetLayerMixerPlayable, 0, 0);
        }

        private void OnDisable()
        {
            if(_blendInCoroutine != null)
                StopCoroutine(_blendInCoroutine);
            
            if(_blendOutCoroutine != null)
                StopCoroutine(_blendOutCoroutine);
            
            foreach (var pair in _playablesPair)
            {
                if (pair.Value.Playable.IsValid())
                    _playableGraph.PlayableGraph.DestroyPlayable(pair.Value.Playable);
            }
            _playablesPair.Clear();
        }
        
        private void Start()
        {
            if (_hasStartAnimations && _onStartAnimations.Length > 0)
                foreach (var animation in _onStartAnimations)
                    Play(animation);
        }
        #endregion
        
        #region Public

        public void Play(AnimationPlayableClipConfig newPlayableConfig)
        {
            if(newPlayableConfig == null)
                return;
            
            if(!_targetLayerMixerPlayable.IsValid())
                return;
            
            if(_playablesPair.Count == 1 && _lastPlayedConfig == newPlayableConfig)
                return;
            
            if (_playablesPair.TryGetValue(newPlayableConfig.Layer + 1, out var oldPlayable))
            {
                if (oldPlayable.Playable.IsValid() &&
                    newPlayableConfig.GetAnimationClip() == oldPlayable.Playable.GetAnimationClip() &&
                    !oldPlayable.ClipConfig.IsStatic)
                    return;

                if(!oldPlayable.ClipConfig.IsStatic && oldPlayable.Playable.IsValid())
                    Stop(oldPlayable.ClipConfig);
            }

            var newAnimationLayer = newPlayableConfig.Layer + 1;
            var newAnimationPlayable = CreatePlayableClip(newPlayableConfig);
            if(newAnimationLayer >=  _targetLayerMixerPlayable.GetInputCount())
                _targetLayerMixerPlayable.SetInputCount(_targetLayerMixerPlayable.GetInputCount() + newAnimationLayer);

            if(!_playablesPair.ContainsKey(newAnimationLayer))    
                _targetLayerMixerPlayable.ConnectInput(newAnimationLayer, newAnimationPlayable, 0);
           
            if(newPlayableConfig.Mask != null)
                _targetLayerMixerPlayable.SetLayerMaskFromAvatarMask((uint)newAnimationLayer,newPlayableConfig.Mask);
            
            _playableGraph.RootLayerMixer.SetInputWeight(_layer, 1);
            _playableGraph.Animator.applyRootMotion = newPlayableConfig.ApplyRootMotion;
            
            _targetLayerMixerPlayable.SetLayerAdditive((uint)newAnimationLayer, newPlayableConfig.IsAdditive);
            if (_playablesPair.Count > 0 || _layer != 0)
            {
                BlendIn(newPlayableConfig);
                BlendOut(newPlayableConfig);
            }
            else
                _targetLayerMixerPlayable.SetInputWeight(newAnimationLayer, newPlayableConfig.TargetWeight);
            
            _lastPlayedConfig = newPlayableConfig;
            _playablesPair.TryAdd(newAnimationLayer, new AnimationPlayablePairConfig 
                {ClipConfig = newPlayableConfig, Playable = newAnimationPlayable});
        }

        public void Stop(AnimationPlayableClipConfig config)
        {
            if(_blendInCoroutine != null)
                StopCoroutine(_blendInCoroutine);
            
            if(_blendOutCoroutine != null)
                StopCoroutine(_blendOutCoroutine);
            
            _targetLayerMixerPlayable.SetInputWeight(config.Layer + 1, 0);
            _playableGraph.RootLayerMixer.SetInputWeight(0, 1);
            
            DisconnectOneShot(config);
        }
        
        #endregion

        #region Private

        private void DisconnectOneShot(AnimationPlayableClipConfig config)
        {
            if (_playablesPair.TryGetValue(config.Layer + 1, out var playableConfig))
            {
                if (!playableConfig.Playable.IsValid())
                    return;

                if (!config.IsStatic)
                {
                    if(_lastPlayedConfig == config)
                        _lastPlayedConfig = null;
                    
                    _playableGraph.PlayableGraph.DestroyPlayable(playableConfig.Playable);
                    _targetLayerMixerPlayable.DisconnectInput(config.Layer + 1);
                    _playablesPair.Remove(config.Layer + 1);
                }
            }
        }
        
        private void BlendIn(AnimationPlayableClipConfig config)
        {
            if(config == null)
                return;
                       
            var clip = config.GetAnimationClip();
            var crossFadeTime = config.UseAutoFadeTimeBasedOnClipLength
                ? Mathf.Clamp(clip.length * 0.1f, 0.1f, clip.length * 0.5f)
                : config.CrossFadeTime;
           
            if(config.Layer <= 0 && config.IsStatic)
                return;
            
            _blendInCoroutine = StartCoroutine(BlendCoroutine(crossFadeTime, config.TargetWeight,blend =>
            {
                float weight = Mathf.Lerp(0f, 1f, blend);
                _targetLayerMixerPlayable.SetInputWeight(config.Layer + 1, weight);
                
            }));
        }
        
        private void BlendOut(AnimationPlayableClipConfig config)
        {
            if(config == null)
                return;
            
            var clip = config.GetAnimationClip();
            var crossFadeTime = config.UseAutoFadeTimeBasedOnClipLength
                ? Mathf.Clamp(clip.length * 0.1f, 0.1f, clip.length * 0.5f)
                : config.CrossFadeTime;
            var delayTime = config.UseAutoFadeTimeBasedOnClipLength
                ? clip.length - crossFadeTime
                : config.FadeDelay;
            
            if(config.Layer <= 0 && config.IsStatic)
                return;
            
            _blendInCoroutine = StartCoroutine(BlendCoroutine(crossFadeTime, config.TargetWeight,blend =>
            {
                float weight = Mathf.Lerp(1f, 0, blend);
                _targetLayerMixerPlayable.SetInputWeight(config.Layer + 1, weight);
                
            }, delayTime, () => DisconnectOneShot(config)));
        }
        
        #endregion
        
        #region Coroutines

        private IEnumerator BlendCoroutine(float crossFadeDuration, float targetWeight,
            Action<float> onBlendCallback, float blendDelay = 0,Action onFinish = null)
        {
            if(blendDelay > 0)
                yield return new WaitForSeconds(blendDelay);
            
            float blendTime = 0;
            while (blendTime < targetWeight)
            {
                blendTime += Time.deltaTime / crossFadeDuration;
                onBlendCallback(blendTime);
                yield return null;
            }
            onBlendCallback(targetWeight);
            onFinish?.Invoke();
        }
        
        #endregion
    }
}
