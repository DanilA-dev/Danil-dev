using CustomCharacterController.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CustomCharacterController.Abilities
{
    public class PlayerMovementAbility : BasePlayerCharacterAbility
    {
        #region Fields

        [SerializeField] private float _acceleration;
        [FoldoutGroup("Events")]
        public UnityEvent<float> OnSpeedChanged;
        [FoldoutGroup("Events")]
        public UnityEvent<Vector3> OnMoveInputChanged;
        
        private Vector3 _moveInput;
        private float _currentSpeed;

        #endregion

        #region Monobehaviour

        private void OnDestroy()
        {
            _playerInputProvider.Move -= OnMoveDirectionChanged;
        }

        #endregion

        #region Overrides

        protected override void OnInitialize()
        {
            _playerInputProvider.Move += OnMoveDirectionChanged;
        }

        protected override void OnTickUpdate()
        {
            _isExecuting = _isActive;
            UpdateMovementAcceleration();
            UpdateMovement();
        }
        

        #endregion

        #region Private

        private void UpdateMovementAcceleration()
        {
            if (_moveInput == Vector3.zero)
            {
                _currentSpeed = 0;
                return;
            }
            
            _currentSpeed += _acceleration * Time.deltaTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _playerMovementCore.MaxHorizontalSpeed);
            OnSpeedChanged?.Invoke(_currentSpeed);
        }
        
        private void UpdateMovement()
        {
            var inputDir = _moveInput.normalized;
            if(_moveInput != Vector3.zero)
                inputDir = transform.right * _moveInput.x + transform.forward * _moveInput.z;

            if(_playerMovementCore.GetInputMovementModifier() != Vector3.zero)
                inputDir = _playerMovementCore.GetInputMovementModifier();
            
            _playerMovementCore.HorizontalVelocity = inputDir.normalized * (_currentSpeed * Time.deltaTime);
        }

        #endregion

        #region Listeners

        private void OnMoveDirectionChanged(Vector2 input)
        {
            _moveInput = new Vector3(input.x, 0, input.y);
            OnMoveInputChanged?.Invoke(_moveInput);
        }

        #endregion
        
    }
}