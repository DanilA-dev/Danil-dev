using UnityEngine;
using UnityEngine.InputSystem;

namespace D_dev.Actions
{
    [System.Serializable]
    public class InputToggleAction : BaseAction
    {
        #region Fields

        [SerializeField] private InputActionReference _inputAction;
        [SerializeField] private bool _enable = true;

        #endregion

        #region IAction

        public override void Execute()
        {
            if (_inputAction?.action == null)
                return;

            if (_enable)
                _inputAction.action.Enable();
            else
                _inputAction.action.Disable();

            IsFinished = true;
        }

        #endregion
    }
}
