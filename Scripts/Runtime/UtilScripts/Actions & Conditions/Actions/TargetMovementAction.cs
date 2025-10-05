using D_Dev.Mover;
using UnityEngine;

namespace D_dev.Actions
{
    [System.Serializable]
    public class TargetMovementAction : IAction
    {
        #region Fields

        [SerializeReference] private IMoverStrategy _movement;
        [Space]
        [SerializeField] private TargetInfo.TargetInfo _target;
        [SerializeField] private float _speed;
        [SerializeField] private float _reachDistance;

        private bool _isFinished;
        
        #endregion

        #region Properties

        public bool IsFinished => _isFinished;

        #endregion

        #region IAction

        public void Execute()
        {
            if (_movement == null)
                return;

            var target = _target.GetTargetPosition();
            _movement.MoveTowards(target, _speed, Time.deltaTime);
            if (_movement.IsAtPosition(target, _reachDistance))
                _isFinished = true;
        }

        #endregion
    }
}
