using UnityEngine;
using UnityEngine.UI;

namespace D_dev.Scripts.ScenarioSystem
{
    public class UIButtonExecutableNode : BaseScenarioExecutableNode
    {
        #region Fields

        [SerializeField] private Button _button;
        private bool _isSubscribed;

        #endregion

        #region Overrides

        public override void Execute()
        {
            if (_button == null)
                return;

            if (!_isSubscribed)
            {
                _button.onClick.AddListener(ForceFinish);
                _isSubscribed = true;
            }
        }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            if (_isSubscribed && _button != null)
            {
                _button.onClick.RemoveListener(ForceFinish);
                _isSubscribed = false;
            }
        }

        #endregion
    }
}