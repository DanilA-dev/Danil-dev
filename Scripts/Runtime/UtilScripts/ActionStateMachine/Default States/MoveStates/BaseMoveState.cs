using D_Dev.Mover;
using UnityEngine;

namespace D_dev.UltimateStateController.Default_States.MoveStates
{
    public abstract class BaseMoveState : BaseState
    {
        #region Fields

        [SerializeReference] protected IMoverStrategy _movement;
        [SerializeField] protected float _speed;
        [SerializeField] protected float _reachDistance;

        #endregion

        #region Abstract / Virtual

        protected abstract Vector3 GetTargetPosition();
        
        protected virtual void OnTargetReached() {}

        #endregion

        #region Overrides

        public override void OnUpdate()
        {
            if (_movement == null) 
                return;

            var target = GetTargetPosition();
            _movement.MoveTowards(target, _speed, Time.deltaTime);

            if (_movement.IsAtPosition(target, _reachDistance))
                OnTargetReached();
        }

        #endregion
    }
}