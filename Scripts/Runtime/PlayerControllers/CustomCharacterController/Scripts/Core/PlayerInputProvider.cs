using System;
using CustomCharacterController.Interfaces;
using D_dev.Scripts.Runtime.UtilScripts.InputSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomCharacterController.Core
{
    public class PlayerInputProvider : MonoBehaviour, IPlayerInputProvider
    {
        #region Fields

        [SerializeField, Required] private InputRouter _inputRouter;
        
        [FoldoutGroup("Debug")]
        [SerializeField, DisplayAsString] private Vector2 _moveInput = Vector2.zero;
        [FoldoutGroup("Debug")]
        [SerializeField, DisplayAsString] private Vector2 _lookInput = Vector2.zero;
        [FoldoutGroup("Debug")]
        [SerializeField, DisplayAsString] private bool _jumpPressed;
        [FoldoutGroup("Debug")]
        [SerializeField, DisplayAsString] private bool _sprintPressed = false;
        [FoldoutGroup("Debug")]
        [SerializeField, DisplayAsString] private bool _crouchPressed = false;
        [FoldoutGroup("Debug")]
        [SerializeField, DisplayAsString] private bool _escPressed = false;
        [FoldoutGroup("Debug")]
        [SerializeField, DisplayAsString] private bool _interactPressed = false;
        [FoldoutGroup("Debug")]
        [SerializeField, DisplayAsString] private bool _rmbPressed = false;
        [FoldoutGroup("Debug")]
        [SerializeField, DisplayAsString] private bool _lmbPressed = false;
        
        public event Action<Vector2> Move;
        public event Action<Vector2> Look;
        public event Action<bool> JumpPressed;
        public event Action<bool> SprintPressed;
        public event Action<bool> CrouchPressed;
        public event Action<bool> EscPressed;
        public event Action<bool> InteractPressed;
        public event Action<bool> RmbPressed;
        public event Action<bool> LmbPressed;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            _inputRouter?.Enable();
            _inputRouter.Move += OnMove;
            _inputRouter.Look += OnLook;
            _inputRouter.JumpPressed += OnJumpPressed;
            _inputRouter.SprintPressed += OnSprintPressed;
            _inputRouter.CrouchPressed += OnCrouchPressed;
            _inputRouter.EscPressed += OnEscPressed;
            _inputRouter.InteractPressed += OnInteractPressed;
            _inputRouter.RmbPressed += OnRmbPressed;
            _inputRouter.LmbPressed += OnLmbPressed;
        }

        private void OnDisable()
        {
            _inputRouter?.Disable();
            _inputRouter.Move -= OnMove;
            _inputRouter.Look -= OnLook;
            _inputRouter.JumpPressed -= OnJumpPressed;
            _inputRouter.SprintPressed -= OnSprintPressed;
            _inputRouter.CrouchPressed -= OnCrouchPressed;
            _inputRouter.EscPressed -= OnEscPressed;
            _inputRouter.InteractPressed -= OnInteractPressed;
            _inputRouter.RmbPressed -= OnRmbPressed;
        }

        #endregion

        #region Listeners

        private void OnMove(Vector2 value)
        {
            _moveInput = value;
            Move?.Invoke(value);
        }

        private void OnLook(Vector2 value)
        {
            _lookInput = value;
            Look?.Invoke(value);
        }

        private void OnJumpPressed(bool value)
        {
            _jumpPressed = value;
            JumpPressed?.Invoke(value);
        }

        private void OnSprintPressed(bool value)
        {
            _sprintPressed = value;
            SprintPressed?.Invoke(value);
        }

        private void OnCrouchPressed(bool value)
        {
            _crouchPressed = value;
            CrouchPressed?.Invoke(value);
        }

        private void OnEscPressed(bool value)
        {
            _escPressed = value;
            EscPressed?.Invoke(value);
        }

        private void OnInteractPressed(bool value)
        {
            _interactPressed = value;
            InteractPressed?.Invoke(value);
        }

        private void OnRmbPressed(bool value)
        {
            _rmbPressed = value;
            RmbPressed?.Invoke(value);
        }

        private void OnLmbPressed(bool value)
        {
            _lmbPressed = value;
            LmbPressed?.Invoke(value);
        }

        #endregion
    }
}