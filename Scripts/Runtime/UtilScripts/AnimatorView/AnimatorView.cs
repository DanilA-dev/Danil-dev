using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

namespace Danil_dev.Scripts.Runtime.UtilScripts.AnimatorView
{
    public class AnimatorView : MonoBehaviour
    {
        #region Fields
        
        [field: SerializeField] public Animator Animator { get; set; }
        
        [FoldoutGroup("Events")]
        public UnityEvent<string> OnAnimationChange;
        
        protected Dictionary<AnimationConfig, int> _animations = new();
        protected Dictionary<string, int> _animationParams = new();

        #endregion

        #region Public

        public void PlayAnimation(AnimationConfig animationConfig)
        {
            if(Animator == null)
                return;
            
            if (_animations.TryGetValue(animationConfig, out var animationHash))
            {
                Animator.CrossFadeInFixedTime(animationHash, animationConfig.CrossFadeTime, animationConfig.Layer);
                OnAnimationChange?.Invoke(animationConfig.AnimationClip.name);
                return;
            }
            var newAnimationHash = Animator.StringToHash(animationConfig.AnimationClip.name);
            Animator.CrossFadeInFixedTime(newAnimationHash, animationConfig.CrossFadeTime, animationConfig.Layer);
            _animations.Add(animationConfig, newAnimationHash);
        }

        public void SetBool(string boolParameter, bool value)
        {
            if(Animator == null)
                return;
            
            if(_animationParams.TryGetValue(boolParameter, out var hash))
                Animator.SetBool(hash, value);
            else
            {
                var newHash = Animator.StringToHash(boolParameter);
                _animationParams.Add(boolParameter, newHash);
                Animator.SetBool(newHash, value);
            }
        }
        
        public void SetFloat(string floatParameter, float value)
        {
            if(Animator == null)
                return;
            
            if(_animationParams.TryGetValue(floatParameter, out var hash))
                Animator.SetFloat(hash, value);
            else
            {
                var newHash = Animator.StringToHash(floatParameter);
                _animationParams.Add(floatParameter, newHash);
                Animator.SetFloat(newHash, value);
            }
        }
        
        public void SetInt(string intParameter, int value)
        {
            if(Animator == null)
                return;
            
            if(_animationParams.TryGetValue(intParameter, out var hash))
                Animator.SetInteger(hash, value);
            else
            {
                var newHash = Animator.StringToHash(intParameter);
                _animationParams.Add(intParameter, newHash);
                Animator.SetInteger(newHash, value);
            }
        }
        
        public void SetTrigger(string triggerParameter)
        {
            if(Animator == null)
                return;
            
            if(_animationParams.TryGetValue(triggerParameter, out var hash))
                Animator.SetTrigger(hash);
            else
            {
                var newHash = Animator.StringToHash(triggerParameter);
                _animationParams.Add(triggerParameter, newHash);
                Animator.SetTrigger(newHash);
            }
        }
        
        public void ChangeAnimatorUpdateMode(AnimatorUpdateMode newUpdateMode)
        {
            if(Animator == null)
                return;
            
            Animator.updateMode = newUpdateMode;
        }

        public void StopAnimator() => Animator?.StopPlayback();

        #endregion
        
        
    }
}