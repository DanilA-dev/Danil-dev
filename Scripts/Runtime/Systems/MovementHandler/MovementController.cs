using UnityEngine;

namespace D_Dev.MovementHandler
{
    public abstract class MovementController : MonoBehaviour
    {
        #region Fields

        [SerializeReference] private BaseMovementHandler _movementHandler;

        #endregion

        #region Abstract

        public abstract Vector3 GetMovementDirection();

        #endregion
        
        #region Public

        public void ApplyMovement(float acceleration, float maxVelocity)
        {
            _movementHandler.ApplyMovement(GetMovementDirection(), acceleration, maxVelocity);
        }
        
        public void StopMovement()
        {
            _movementHandler.StopMovement();
        }
        
        public float GetVelocity()
        {
            return _movementHandler.GetVelocity();
        }
        
        public bool IsMoving()
        {
            return _movementHandler.IsMoving();
        }

        #endregion
    }
}