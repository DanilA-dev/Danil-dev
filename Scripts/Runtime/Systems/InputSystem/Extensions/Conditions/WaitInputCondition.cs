using D_Dev.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace D_Dev.InputSystem.Extensions.Conditions
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

        private bool _isMet;
        private bool _isSubscribed;

        #endregion

        #region ICondition

        public bool IsConditionMet()
        {
            if (!_isSubscribed)
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
                _isSubscribed = true;
            }
            return _isMet;
        }

        public void Reset()
        {
            _isMet = false;
        }

        #endregion

        #region Listeners

        private void OnInputTriggered(InputAction.CallbackContext context)
        {
            _isMet = true;
        }

        #endregion
    }
}
