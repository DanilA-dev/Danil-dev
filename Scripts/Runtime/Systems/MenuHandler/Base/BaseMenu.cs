using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
#if DOTWEEN
using DG.Tweening;
using D_Dev.TweenAnimations.Types;
#endif

namespace D_Dev.MenuHandler
{
    public class BaseMenu : MonoBehaviour
    {
        #region Fields

        [OnValueChanged(nameof(ActiveColor))]
        [GUIColor(nameof(ActiveColor))]
        [SerializeField, ReadOnly] private bool _isOpen;
        [Title("Animations")] 
        [SerializeField] private bool _hasOpenAnimation;
        [SerializeField] private bool _hasCloseAniation;
        [ShowIf(nameof(_hasCloseAniation))]
        [SerializeField] private bool _disableObjectOnComplete;
#if DOTWEEN
        [ShowIf(nameof(_hasOpenAnimation))] 
        [SerializeReference] private List<BaseAnimationTween> _openAnimations = new();

        [ShowIf(nameof(_hasCloseAniation))] 
        [SerializeReference] private List<BaseAnimationTween> _closeAnimations = new();
#endif
        [FoldoutGroup("Events")]
        public UnityEvent OnOpenEvent;
        [FoldoutGroup("Events")]
        public UnityEvent OnCloseEvent;
        
        #endregion

        #region Properties
        public bool IsOpen => _isOpen;

        #endregion

        #region Monobehaviour

        protected void Awake() => ForceClose();

        #endregion

        #region Public

        public async void Open()
        {
            if(IsOpen)
                return;
            
            gameObject.SetActive(true);
#if DOTWEEN
            if (_hasOpenAnimation && _openAnimations != null
                                  && _openAnimations.Count > 0)
            {
                var seq = DOTween.Sequence();
                seq.Restart();
                seq.SetAutoKill(gameObject);
               
                foreach (var openAnimation in _openAnimations)
                    seq.Append(openAnimation.Play());

                await seq.AsyncWaitForCompletion().AsUniTask();
                _isOpen = true;
                return;
            }
#endif
            _isOpen = true;
            OnOpenEvent?.Invoke();
        }

        public async void Close()
        {
            if(!IsOpen)
                return;
            
#if DOTWEEN
            if (_hasCloseAniation && _closeAnimations != null
                                  && _closeAnimations.Count > 0)
            {
                var seq = DOTween.Sequence();
                seq.Restart();
                seq.SetAutoKill(gameObject);
                
                foreach (var closeAnimation in _closeAnimations)
                    seq.Append(closeAnimation.Play());

                await seq.AsyncWaitForCompletion().AsUniTask();
                _isOpen = false;
                gameObject.SetActive(!_disableObjectOnComplete);
                OnCloseEvent?.Invoke();
                return;
            }
#endif
            ForceClose();
        }

        public void ForceOpen()
        {
            _isOpen = true;
            gameObject.SetActive(IsOpen);
        }

        public void ForceClose()
        {
            _isOpen = false;
            gameObject.SetActive(IsOpen);
            OnCloseEvent?.Invoke();
        }

        #endregion

        #region Virtual
        protected virtual void OnOpen() {}
        protected virtual void OnClose() {}

        #endregion

        #region Private

        private Color ActiveColor() => _isOpen ? Color.green : Color.red;

        #endregion
    }
}
