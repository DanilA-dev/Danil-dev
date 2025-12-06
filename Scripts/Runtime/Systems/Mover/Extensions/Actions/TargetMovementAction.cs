using D_Dev.Base;
using D_Dev.PositionRotationConfig;
using UnityEngine;

namespace D_Dev.Mover.Extensions.Actions
{
    [System.Serializable]
    public class TargetMovementAction : BaseAction
    {
        #region Fields

        [SerializeReference] private IMoverStrategy _movement;
        [Space]
        [SerializeReference] private BasePositionSettings _target = new();
        [SerializeField] private float _speed;
        [SerializeField] private float _reachDistance;

        #endregion

        #region Overrides

        public override void Execute()
        {
            if (_movement == null)
                return;

            var target = _target.GetPosition();
            _movement.MoveTowards(target, _speed, Time.deltaTime);
            if (_movement.IsAtPosition(target, _reachDistance))
                IsFinished = true;
        }

        #endregion
    }
}
