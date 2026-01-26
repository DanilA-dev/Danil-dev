using D_Dev.PolymorphicValueSystem;
using D_Dev.Utility;
using UnityEngine;

namespace D_Dev.PlayerStateController
{
    public class PlayerMovementHandler : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeReference] private PolymorphicValue<Transform> _directionRoot;
        [SerializeReference] private PolymorphicValue<Vector3> _rawInputDirection;
        [SerializeReference] private PolymorphicValue<Vector3> _movementDirection;
        
        private RotationHandler _rotationHandler;
        private RigidbodyMovementHandler _movementHandler;
        
        #endregion
        
        #region Properties
        
        public RotationHandler RotationHandler => _rotationHandler;
        public RigidbodyMovementHandler MovementHandler => _movementHandler;
        
        #endregion
        
        #region Monobehaviour
        private void Awake()
        {
            _rotationHandler = new RotationHandler();
            _rotationHandler.Initialize(transform);
            
            _movementHandler = new RigidbodyMovementHandler();
            _movementHandler.Initialize(_rigidbody);
        }

        private void Update() => UpdateMovementDirection();

        #endregion

        #region Helpers

        public void ApplyMovement(float acceleration, float maxSpeed, ForceMode forceMode = ForceMode.Acceleration)
        {
            _movementHandler.ApplyMovement(_movementDirection.Value, acceleration, maxSpeed, forceMode);
        }
        
        public void StopMovement()
        {
            _movementHandler.StopMovement();
        } 
        public void ApplyBraking(float deceleration = 10f, ForceMode forceMode = ForceMode.Acceleration)
        {
            _movementHandler.ApplyBraking(deceleration, forceMode);
        }
        
        public float GetCurrentVelocityMagnitude()
        {
            return _movementHandler.GetCurrentVelocityMagnitude();
        }
        
        public bool IsMoving()
        {
            return _movementHandler.IsMoving();
        }
        
        public void RotateTowards(Vector3 direction, float rotationSpeed = -1f)
        {
            _rotationHandler.RotateTowards(direction, rotationSpeed);
        }
        
        public void FaceMovementDirection(float rotationSpeed = -1f)
        {
            _rotationHandler.FaceMovementDirection(_movementDirection.Value, rotationSpeed);
        }
        
        public void InstantRotate(Vector3 direction)
        {
            _rotationHandler.InstantRotate(direction);
        }
        
        public bool IsRotationComplete(Vector3 targetDirection, float tolerance = 5f)
        {
            return _rotationHandler.IsRotationComplete(targetDirection, tolerance);
        }
        
        public Vector3 GetCurrentForwardDirection()
        {
            return _rotationHandler.GetCurrentForwardDirection();
        }
        
        public void SetRotationSpeed(float rotationSpeed)
        {
            _rotationHandler.SetRotationSpeed(rotationSpeed);
        }

        #endregion
        
        #region Private

        private void UpdateMovementDirection()
        {
            var rootRight = _directionRoot.Value.right;
            var rootForward = _directionRoot.Value.forward;
            rootRight.y = 0f;
            rootForward.y = 0f;
            
            _movementDirection.Value = rootRight * _rawInputDirection.Value.x + rootForward * _rawInputDirection.Value.z;
        }   

        #endregion
    }
}