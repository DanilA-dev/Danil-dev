using UnityEngine;
using UnityEngine.AI;

namespace D_Dev.MovementHandler
{
    [System.Serializable]
    public class NavMeshMovementHandler : BaseMovementHandler
    {
        #region Fields

        [SerializeField] private NavMeshAgent _navMeshAgent;

        #endregion

        #region Overrides

        public override void ApplyMovement(Vector3 direction, float acceleration, float maxVelocity)
        {
            _direction = direction;
            if (_navMeshAgent == null)
                return;

            _navMeshAgent.speed = maxVelocity;
            _navMeshAgent.destination = direction;
        }

        public override void StopMovement()
        {
            if (_navMeshAgent == null)
                return;

            _navMeshAgent.ResetPath();
            _direction = Vector3.zero;
        }

        public override float GetVelocity() => _navMeshAgent != null ? _navMeshAgent.velocity.magnitude : 0f;
        public override bool IsMoving() => _navMeshAgent != null && _navMeshAgent.velocity.magnitude > 0.1f;

        #endregion
    }
}