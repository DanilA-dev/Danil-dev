using UnityEngine;
using UnityEngine.InputSystem;

namespace D_dev.Conditions
{
    [System.Serializable]
    public class WaitInputCondition : ICondition
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
        [SerializeField] private bool _resetOnMet = true;

        private bool _isMet;

        #endregion

        #region ICondition

        public bool IsConditionMet()
        {
            return _isMet;
        }

        public void Reset()
        {
            _isMet = false;
        }

        #endregion

        #region Public

        public void Enable()
        {
            if (_inputAction?.action != null)
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
            }
        }

        public void Disable()
        {
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
            }
        }

        #endregion

        #region Listeners

        private void OnInputTriggered(InputAction.CallbackContext context)
        {
            _isMet = true;
            if (_resetOnMet)
            {
                // Optionally disable after met
            }
        }

        #endregion
    }
}
