using UnityEngine;
using UnityEngine.InputSystem;

namespace D_Dev.ScenarioSystem
{
    public class InputPerformExecutableNode : BaseScenarioExecutableNode
    {
        #region Fields

        [SerializeField] private InputActionReference _inputAction;
        [SerializeField] private InputActionPhase desiredPhase = InputActionPhase.Performed;
        private bool _isSubscribed;

        #endregion

        #region Monobehaviour

        public void OnEnable()
        {
        }
        
        private void OnDestroy()
        {
            if (_isSubscribed && _inputAction?.action != null)
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
                _isSubscribed = false;
            }
        }

        #endregion

        #region Overrides

        public override void Execute()
        {
            if (_inputAction?.action == null)
                return;

            if (!_isSubscribed)
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
                _isSubscribed = true;
            }
        }

        #endregion

        #region Listeners

        private void OnInputTriggered(InputAction.CallbackContext context)
        {
            IsFinished = true;
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

        #endregion
    }
}
