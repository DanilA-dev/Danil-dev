using UnityEngine;
using UnityEngine.InputSystem;

namespace D_dev.Actions
{
    [System.Serializable]
    public class InputToggleAction : IAction
    {
        #region Fields

        [SerializeField] private InputActionReference _inputAction;
        [SerializeField] private bool _enable = true;

        private bool _isFinished;

        #endregion

        #region IAction

        public bool IsFinished => _isFinished;

        public void Execute()
        {
            if (_inputAction?.action == null)
                return;

            if (_enable)
                _inputAction.action.Enable();
            else
                _inputAction.action.Disable();

            _isFinished = true;
        }

        public void Undo()
        {
            _isFinished = false;
            // Optionally toggle back, but for simplicity, just reset
        }

        #endregion
    }
}
