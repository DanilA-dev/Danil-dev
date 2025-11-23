using D_Dev.Mover;
using UnityEngine;

namespace D_dev.Actions
{
    [System.Serializable]
    public class TargetMovementAction : BaseAction
    {
        #region Fields

        [SerializeReference] private IMoverStrategy _movement;
        [Space]
        [SerializeField] private TargetInfo.TargetInfo _target;
        [SerializeField] private float _speed;
        [SerializeField] private float _reachDistance;

        #endregion

        #region IAction

        public override void Execute()
        {
            if (_movement == null)
                return;

            var target = _target.GetTargetPosition();
            _movement.MoveTowards(target, _speed, Time.deltaTime);
            if (_movement.IsAtPosition(target, _reachDistance))
                IsFinished = true;
        }

        #endregion
    }
}
