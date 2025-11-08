using CustomCharacterController.Core;
using D_Dev.InteractableSystem.InteractableDetector;
using UnityEngine;

namespace CustomCharacterController.Abilities
{
    public class PlayerInteractAbility : BasePlayerCharacterAbility
    {
        #region Fields

        [SerializeField] private InteractableDetector _interactableDetector;

        #endregion
        
        #region Overrides

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _playerInputProvider.InteractPressed += OnInteractPressed;
        }

        #endregion

        #region Listeners

        private void OnInteractPressed(bool isPressed)
        {
            if(!IsActive)
                return;
            
            IsExecuting = isPressed;
            if (isPressed)
                _interactableDetector.CurrentInteractable?.StartInteract(gameObject);
            else
                _interactableDetector.CurrentInteractable?.StopInteract(gameObject);
        }

        #endregion
    }
}