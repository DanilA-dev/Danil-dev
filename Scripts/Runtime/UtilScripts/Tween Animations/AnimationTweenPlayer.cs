#if DOTWEEN
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.UtilScripts.Tween_Animations
{
    public class AnimationTweenPlayer : BaseAnimationTweenPlayable
    {
        #region Enums

        public enum PlayMode
        {
            PlayAll = 0,
            PlayAtIndex = 1
        }

        #endregion

        #region Fields

        [PropertyOrder(-10)]
        [ShowIf("@_playOnEnable || _playOnStart")]
        [SerializeField] private PlayMode _playMode;
        [ShowIf("@_playMode == PlayMode.PlayAtIndex")]
        [PropertyOrder(-10)]
        [SerializeField] private int _startIndex;

        #endregion

        #region Monobehaviour

        protected override void OnEnable()
        {
            if (_playOnEnable && HasTweensInArray())
            {
                if (_playMode == PlayMode.PlayAll)
                    Play();
                else
                    Play(_startIndex);
            }
        }

        protected override void Start()
        {
            if (_playOnStart && HasTweensInArray())
            {
                if (_playMode == PlayMode.PlayAll)
                    Play();
                else
                    Play(_startIndex);
            }
        }

        #endregion

        #region Override

        protected override void OnPlay()
        {
            if(!HasTweensInArray())
                return;
            
            var seq = DOTween.Sequence();

            foreach (var tween in _tweens)
                seq.Join(tween.Play());
            

            seq.SetAutoKill(gameObject);
        }

        public override void Pause()
        {
            if (!HasTweensInArray())
                return;

            foreach (var tween in _tweens)
                tween.Pause();
        }

        #endregion

        #region Public

        public void Play(int index)
        {
            if (!HasTweensInArray() || index < 0 || index >= _tweens.Length)
                return;
            OnStart?.Invoke();
            _tweens[index].Play();
            IsComplete = false;
            _tweens[index].OnComplete.AddListener(() =>
            {
                OnComplete?.Invoke();
                IsComplete = true;
                _tweens[index].OnComplete.RemoveAllListeners();
            });
        }

        #endregion
    }
}
#endif
