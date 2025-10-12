using UnityEngine;
using UnityEngine.InputSystem;

namespace D_dev.Actions
{
    [System.Serializable]
    public class WaitInputAction : IAction
    {
        #region Enums

        public enum InputActionPhase
        {
            Started,
            Performed,
            Canceled
        }

        #endregion

        #region Fields

        [SerializeField] private InputActionReference _inputAction;
        [SerializeField] private InputActionPhase _desiredPhase = InputActionPhase.Performed;

        private bool _isFinished;
        private bool _isSubscribed;

        #endregion

        #region IAction

        public bool IsFinished => _isFinished;

        public void Execute()
        {
            if (_inputAction?.action == null)
                return;

            if (!_isSubscribed)
            {
                _inputAction.action.Enable();
                switch (_desiredPhase)
                {
                    case InputActionPhase.Started:
                        _inputAction.action.started += OnInputTriggered;
                        break;
                    case InputActionPhase.Performed:
                        _inputAction.action.performed += OnInputTriggered;
                        break;
                    case InputActionPhase.Canceled:
                        _inputAction.action.canceled += OnInputTriggered;
                        break;
                }
                _isSubscribed = true;
            }
        }

        public void Undo()
        {
            _isFinished = false;
            if (_isSubscribed && _inputAction?.action != null)
            {
                switch (_desiredPhase)
                {
                    case InputActionPhase.Started:
                        _inputAction.action.started -= OnInputTriggered;
                        break;
                    case InputActionPhase.Performed:
                        _inputAction.action.performed -= OnInputTriggered;
                        break;
                    case InputActionPhase.Canceled:
                        _inputAction.action.canceled -= OnInputTriggered;
                        break;
                }
                _inputAction.action.Disable();
                _isSubscribed = false;
            }
        }

        #endregion

        #region Listeners

        private void OnInputTriggered(InputAction.CallbackContext context)
        {
            _isFinished = true;
            if (_inputAction?.action != null)
            {
                switch (_desiredPhase)
                {
                    case InputActionPhase.Started:
                        _inputAction.action.started -= OnInputTriggered;
                        break;
                    case InputActionPhase.Performed:
                        _inputAction.action.performed -= OnInputTriggered;
                        break;
                    case InputActionPhase.Canceled:
                        _inputAction.action.canceled -= OnInputTriggered;
                        break;
                }
                _inputAction.action.Disable();
                _isSubscribed = false;
            }
        }

        #endregion
    }
}
