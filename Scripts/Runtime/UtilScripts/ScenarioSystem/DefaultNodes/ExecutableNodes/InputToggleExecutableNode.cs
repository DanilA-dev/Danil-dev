using UnityEngine;
using UnityEngine.InputSystem;

namespace D_dev.Scripts.ScenarioSystem
{
    public class InputToggleExecutableNode : BaseScenarioExecutableNode
    {
        #region Fields

        [SerializeField] private InputActionReference _inputAction;
        [SerializeField] private bool _enable = true;

        private bool _isToggled;

        #endregion

        #region Overrides

        public override void Execute()
        {
            if (_inputAction?.action == null)
                return;

            if (!_isToggled)
            {
                if (_enable)
                    _inputAction.action.Enable();
                else
                    _inputAction.action.Disable();
                
                _isToggled = true;
                IsFinished = true;
            }
        }

        #endregion
    }
}