using CustomCharacterController.Core;
using D_Dev.TimerSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomCharacterController.Abilities
{
    public class PlayerGravityAbility : BasePlayerCharacterAbility
    {
        #region Fields

        [Title("Gravity Settings")]
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _fallTerminalVelocity = -53f;
        
        [Title("Fall Settings")]
        [SerializeField] private float _fallTimeout = 0.15f;
        
        [Space]
        [FoldoutGroup("Debug")]
        [SerializeField, DisplayAsString] private bool _isFalling;
        [FoldoutGroup("Debug")]
        [SerializeField, DisplayAsString] private float _currentVerticalVelocity;
        
        
        private CountdownTimer _fallTimer;
        private float _verticalVelocity;

        #endregion

        #region Properties
        public float VerticalVelocity => _verticalVelocity;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            InitializeTimers();
        }

        #endregion

        #region Overrides

        protected override void OnInitialize()
        {
            _playerMovementCore.Gravity = _gravity;
        }

        protected override void OnTickUpdate()
        {
            _fallTimer.Tick(Time.deltaTime);
            
            SyncVerticalVelocity();
            UpdateGravity();
            ApplyVerticalVelocity();
            UpdateDebugInfo();
        }

        #endregion

        #region Private

        private void InitializeTimers()
        {
            _fallTimer = new CountdownTimer(_fallTimeout);
            _fallTimer.OnTimerEnd += () =>
            {
                IsExecuting = true;
                _isFalling = IsExecuting;
            };
        }

        private void UpdateGravity()
        {
            if (_playerMovementCore.IsGrounded)
            {
                ResetFallState();
                
                if (_verticalVelocity < 0)
                    _verticalVelocity = -2f; 
            }
            else
            {
                if (!IsExecuting && !_fallTimer.IsRunning)
                    _fallTimer.Start();

                if (_verticalVelocity > _fallTerminalVelocity)
                    _verticalVelocity += _gravity * Time.deltaTime;
            }
        }

        private void SyncVerticalVelocity() => _verticalVelocity = _playerMovementCore.GetVerticalVelocity();

        private void ApplyVerticalVelocity() => _playerMovementCore.SetVerticalVelocity(_verticalVelocity);

        private void UpdateDebugInfo() => _currentVerticalVelocity = _verticalVelocity;
        private void ResetFallState()
        {
            if(!IsExecuting)
                return;
            
            IsExecuting = false;
            _isFalling = IsExecuting;
            _fallTimer.Reset();
        }


        #endregion
    }
}