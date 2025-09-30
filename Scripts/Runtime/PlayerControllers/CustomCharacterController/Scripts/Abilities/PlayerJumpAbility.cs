using CustomCharacterController.Core;
using D_Dev.TimerSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomCharacterController.Abilities
{
    public class PlayerJumpAbility : BasePlayerCharacterAbility
    {
        #region Fields

        [Title("Jump Settings")]
        [SerializeField] private float _jumpHeight = 1.2f;
        [SerializeField] private float _jumpTimeout = 0.50f;
        [SerializeField] private bool _canJumpInAir;

        [Space]
        [FoldoutGroup("Debug")]
        [SerializeField, DisplayAsString] private bool _isJumping;
        
        private CountdownTimer _jumpTimer;
        private bool _jumpInput;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            InitializeTimers();
        }

        private void OnDestroy()
        {
            _playerInputProvider.JumpPressed -= OnJumpPressed;
        }

        #endregion

        #region Overrides

        protected override void OnInitialize()
        {
            _playerInputProvider.JumpPressed += OnJumpPressed;
        }

        protected override void OnTickUpdate()
        {
            _jumpTimer.Tick(Time.deltaTime);
            ProcessJumpInput();
        }

        #endregion

        #region Private

        private void InitializeTimers()
        {
            _jumpTimer = new CountdownTimer(_jumpTimeout);
        }

        private void ProcessJumpInput()
        {
            if (_jumpInput)
            {
                PerformJump();
                _jumpInput = false;
            }
            
            if (!_playerMovementCore.IsGrounded)
            {
                _isJumping = false;
            }
        }

        private void PerformJump()
        {
            float jumpVelocity = Mathf.Sqrt(_jumpHeight * -2f * _playerMovementCore.Gravity);
            _playerMovementCore.AddVerticalImpulse(jumpVelocity);
            
            _jumpTimer.Start();
            _isJumping = true;
        }

        #endregion

        #region Listeners

        private void OnJumpPressed(bool isJump)
        {
            if (!_canJumpInAir && !_playerMovementCore.IsGrounded || IsBlockedByExecutingAbilities())
                return;
            
            _jumpInput = isJump;
            IsExecuting = isJump && IsActive;
        }

        #endregion
    }
}