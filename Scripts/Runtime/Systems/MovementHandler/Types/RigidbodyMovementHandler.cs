using UnityEngine;

namespace D_Dev.MovementHandler
{
    [System.Serializable]
    public class RigidbodyMovementHandler : BaseMovementHandler
    {
        #region Fields

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ForceMode _forceMode;

        #endregion

        #region Overrides

        public override void ApplyMovement(Vector3 direction, float acceleration, float maxVelocity)
        {
            _direction = direction;

            if (_rigidbody == null)
                return;
 
            _direction = direction;
            if (direction.magnitude > 0.1f)
            {
                Vector3 force = direction.normalized * acceleration;
                _rigidbody.AddForce(force, _forceMode);
            }

            if (_rigidbody.linearVelocity.magnitude > maxVelocity)
                _rigidbody.linearVelocity = Vector3.ClampMagnitude(_rigidbody.linearVelocity, maxVelocity);
        }

        public override void StopMovement()
        {
            if (_rigidbody == null)
                return;
 
            _rigidbody.linearVelocity = Vector3.zero;
            _direction = Vector3.zero;
        }

        public override float GetVelocity() => _rigidbody != null ? _rigidbody.linearVelocity.magnitude : 0f;
        public override bool IsMoving() => _rigidbody != null && _rigidbody.linearVelocity.magnitude > 0.1f;

        #endregion
    }
}