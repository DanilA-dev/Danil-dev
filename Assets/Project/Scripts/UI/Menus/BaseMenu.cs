using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TweenAnimations;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public abstract class BaseMenu : MonoBehaviour
    {
        [FormerlySerializedAs("_hasOpenAniation")]
        [Title("Animations")] 
        [SerializeField] private bool _hasOpenAnimation;
        [ShowIf(nameof(_hasOpenAnimation))]
        [SerializeReference] private BaseAnimationTween[] _openAnimations;
        [SerializeField] private bool _hasCloseAniation;
        [ShowIf(nameof(_hasCloseAniation))]
        [SerializeField] private bool _disableObjectOnComplete;
        [ShowIf(nameof(_hasCloseAniation))]
        [SerializeReference] private BaseAnimationTween[] _closeAnimations;
        
        public bool IsOpen { get; private set; }

        protected void Awake()
        {
            ForceClose();
        }

        public async void Open()
        {
            if(IsOpen)
                return;
            
            gameObject.SetActive(true);
            if (_hasOpenAnimation && _openAnimations != null
                && _openAnimations.Length > 0)
            {
                var seq = DOTween.Sequence();
                seq.Restart();
                seq.SetAutoKill(gameObject);
               
                foreach (var openAnimation in _openAnimations)
                    seq.Append(openAnimation.Play());

                await seq.AsyncWaitForCompletion().AsUniTask();
                IsOpen = true;
            }
            else
                IsOpen = true;
           
        }

        public async void Close()
        {
            if(!IsOpen)
                return;
            
            if (_hasCloseAniation && _closeAnimations != null
                && _closeAnimations.Length > 0)
            {
                var seq = DOTween.Sequence();
                seq.Restart();
                seq.SetAutoKill(gameObject);
                
                foreach (var closeAnimation in _closeAnimations)
                    seq.Append(closeAnimation.Play());

                await seq.AsyncWaitForCompletion().AsUniTask();
                IsOpen = false;
                gameObject.SetActive(!_disableObjectOnComplete);
            }
            else
                ForceClose();
        }

        public void ForceOpen()
        {
            IsOpen = true;
            gameObject.SetActive(IsOpen);
        }

        public void ForceClose()
        {
            IsOpen = false;
            gameObject.SetActive(IsOpen);
        }
        
        protected virtual void OnOpen() {}
        protected virtual void OnClose() {}

    }
}