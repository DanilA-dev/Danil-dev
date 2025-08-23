using CustomCharacterController.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CustomCharacterController.Abilities
{
    public class PlayerSprintAbility : BasePlayerCharacterAbility
    {
        #region Fields

        [SerializeField] private float _maxSprintSpeed;
        [FoldoutGroup("Events")]
        public UnityEvent<bool> OnSprint;
        
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
            
            if (!_isActive)
            {
                _playerMovementCore.MaxHorizontalSpeed = _defaultMovementSpeed;
                return;
            }
            
            _playerMovementCore.MaxHorizontalSpeed = isSprint ? _maxSprintSpeed : _defaultMovementSpeed;
            _isExecuting = isSprint;
            OnSprint?.Invoke(_isExecuting);
        }

        #endregion
    }
}