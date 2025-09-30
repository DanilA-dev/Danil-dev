using System;
using UnityEngine;
using UnityEngine.UI;

namespace D_Dev.ScenarioSystem
{
    public class UIButtonBreakNode : BaseScenarioBreakNode
    {
        #region Fields

        [SerializeField] private Button _button;

        private bool _buttonClicked;
        private bool _isSubscribed;

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
        }

        private void OnDestroy()
        {
            if (_isSubscribed && _button != null)
            {
                _button.onClick.RemoveListener(OnButtonClicked);
                _isSubscribed = false;
            }
        }

        #endregion
        
        #region Overrides

        protected override bool CheckBreakCondition()
        {
            if (_button == null)
                return false;

            if (!_isSubscribed)
            {
                _button.onClick.AddListener(OnButtonClicked);
                _isSubscribed = true;
            }

            return _buttonClicked;
        }

        #endregion
        
        #region Listeners

        private void OnButtonClicked()
        {
            _buttonClicked = true;
            if (_button != null)
                _button.onClick.RemoveListener(OnButtonClicked);
        }

        #endregion
    }
}
