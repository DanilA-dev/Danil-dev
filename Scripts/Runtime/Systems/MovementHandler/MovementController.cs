using UnityEngine;

namespace D_Dev.MovementHandler
{
    public abstract class MovementController : MonoBehaviour
    {
        #region Fields

        [SerializeReference] private BaseMovementHandler _movementHandler;

        #endregion

        #region Monobehaviour

        protected virtual void Update() => _movementHandler?.OnUpdate();
        protected virtual void FixedUpdate() => _movementHandler?.OnFixedUpdate();

        #endregion
        
        #region Public

        public void SetDirection(Vector3 direction) => _movementHandler.Direction = direction;
        public void SetMaxVelocity(float maxVelocity) => _movementHandler.MaxVelocity = maxVelocity;
        public void SetAcceleration(float acceleration) => _movementHandler.Acceleration = acceleration;
        public void StopMovement() => _movementHandler.StopMovement();
        public float GetVelocity() => _movementHandler.GetVelocity();
        public bool IsMoving() => _movementHandler.IsMoving();

        #endregion
    }
}