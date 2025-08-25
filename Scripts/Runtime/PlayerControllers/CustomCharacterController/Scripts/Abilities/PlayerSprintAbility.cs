using CustomCharacterController.Core;
using UnityEngine;

namespace CustomCharacterController.Abilities
{
    public class PlayerSprintAbility : BasePlayerCharacterAbility
    {
        #region Fields

        [SerializeField] private float _maxSprintSpeed;
        
        private float _defaultMovementSpeed;

        #endregion

        #region Monobehaviour

        private void OnDestroy()
        {
            _playerInputProvider.SprintPressed -= OnSprintPressed;
        }

        #endregion
        
        #region Overrides

        protected override void OnInitialize()
        {
           _playerInputProvider.SprintPressed += OnSprintPressed;
           _defaultMovementSpeed = _playerMovementCore.MaxHorizontalSpeed;
        }
        

        #endregion

        #region Listeners

        private void OnSprintPressed(bool isSprint)
        {
            if(IsBlockedByExecutingAbilities())
                return;
            
            if (!IsActive)
            {
                _playerMovementCore.MaxHorizontalSpeed = _defaultMovementSpeed;
                return;
            }
            
            _playerMovementCore.MaxHorizontalSpeed = isSprint ? _maxSprintSpeed : _defaultMovementSpeed;
            IsExecuting = isSprint;
        }

        #endregion
    }
}