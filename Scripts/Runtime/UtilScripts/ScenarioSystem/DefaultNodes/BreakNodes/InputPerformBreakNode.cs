using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace D_dev.Scripts.ScenarioSystem
{
    public class InputPerformBreakNode : BaseScenarioBreakNode
    {
        #region Fields

        [SerializeField] private InputActionReference _inputAction;
        [SerializeField] private InputActionPhase desiredPhase = InputActionPhase.Performed;
            
        private bool _inputPerformed;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            if (_inputAction?.action != null)
            {
                _inputAction.action.Enable();
                switch (desiredPhase)
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

        private void OnDestroy()
        {
            if (_inputAction?.action != null)
            {
                switch (desiredPhase)
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
        
        #region Overrides

        protected override bool CheckBreakCondition()
        {
            if (_inputAction?.action == null)
                return false;

            return _inputPerformed;
        }

        #endregion

        #region Listeners

        private void OnInputTriggered(InputAction.CallbackContext context)
        {
            _inputPerformed = true;
            if (_inputAction?.action != null)
            {
                switch (desiredPhase)
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
    }
}