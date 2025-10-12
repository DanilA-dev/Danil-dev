using UnityEngine;
using UnityEngine.UI;

namespace D_dev.Actions
{
    [System.Serializable]
    public class WaitUIButtonAction : IAction
    {
        #region Fields

        [SerializeField] private Button _button;

        private bool _isFinished;
        private bool _isSubscribed;

        #endregion

        #region IAction

        public bool IsFinished => _isFinished;

        public void Execute()
        {
            if (_button == null)
                return;

            if (!_isSubscribed)
            {
                _button.onClick.AddListener(OnButtonClicked);
                _isSubscribed = true;
            }
        }

        public void Undo()
        {
            _isFinished = false;
            if (_isSubscribed && _button != null)
            {
                _button.onClick.RemoveListener(OnButtonClicked);
                _isSubscribed = false;
            }
        }

        #endregion

        #region Listeners

        private void OnButtonClicked()
        {
            _isFinished = true;
            if (_button != null)
                _button.onClick.RemoveListener(OnButtonClicked);
            _isSubscribed = false;
        }

        #endregion
    }
}
