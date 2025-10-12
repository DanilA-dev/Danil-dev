using UnityEngine;

namespace D_dev.Actions
{
    [System.Serializable]
    public class ToggleObjectAction : IAction
    {
        #region Fields

        [SerializeField] private GameObject _targetObject;
        [SerializeField] private bool _enable = true;

        private bool _isFinished;

        #endregion

        #region IAction

        public bool IsFinished => _isFinished;

        public void Execute()
        {
            if (_targetObject != null)
                _targetObject.SetActive(_enable);

            _isFinished = true;
        }

        public void Undo()
        {
            _isFinished = false;
            // Optionally toggle back
            if (_targetObject != null)
                _targetObject.SetActive(!_enable);
        }

        #endregion
    }
}
