using UnityEngine;

namespace D_Dev.MovementHandler
{
    [System.Serializable]
    public abstract class BaseMovementHandler
    {
        #region Fields

        protected Vector3 _direction;

        #endregion

        #region Properties

        public Vector3 Direction => _direction;

        #endregion

        #region Abstract

        public abstract void ApplyMovement(Vector3 direction, float acceleration, float maxVelocity);
        public abstract void StopMovement();
        public abstract float GetVelocity();
        public abstract bool IsMoving();

        #endregion
    }
}