using UnityEngine;

namespace D_Dev.Utility
{
    public class RigidbodyMovementHandler
    {
         #region Fields

         private Rigidbody _rigidbody;
         private Vector3 _direction;
         private float _currentSpeed;
 
         #endregion
 
         #region Properties
 
         public Vector3 Direction => _direction;
         public float CurrentSpeed => _currentSpeed;
 
         #endregion
 
         #region Initialization
 
         public void Initialize(Rigidbody rigidbody)
         {
             _rigidbody = rigidbody;
         }
 
         #endregion
 
         #region Public
        
         public void ApplyMovement(Vector3 direction, float speed, ForceMode forceMode = ForceMode.Acceleration)
         {
             if (_rigidbody == null)
                 return;
 
             _direction = direction;
             _currentSpeed = speed;
 
             if (direction.magnitude > 0.1f)
             {
                 Vector3 force = direction.normalized * speed;
                 _rigidbody.AddForce(force, forceMode);
             }
         }
 
        
         public void StopMovement()
         {
             if (_rigidbody == null)
                 return;
 
             _rigidbody.linearVelocity = Vector3.zero;
             _direction = Vector3.zero;
             _currentSpeed = 0f;
         }
 
        
         public void ApplyBraking(float deceleration = 10f, ForceMode forceMode = ForceMode.Acceleration)
         {
             if (_rigidbody == null)
                 return;
 
             Vector3 currentVelocity = _rigidbody.linearVelocity;
             Vector3 brakeForce = -currentVelocity.normalized * deceleration;
 
             float brakeMagnitude = Mathf.Min(deceleration, currentVelocity.magnitude);
             _rigidbody.AddForce(brakeForce * brakeMagnitude, forceMode);
         }
 
         
         public float GetCurrentVelocityMagnitude()
         {
             return _rigidbody != null ? _rigidbody.linearVelocity.magnitude : 0f;
         }
 
        
         public bool IsMoving()
         {
             return _rigidbody != null && _rigidbody.linearVelocity.magnitude > 0.1f;
         }

        #endregion
    }
}