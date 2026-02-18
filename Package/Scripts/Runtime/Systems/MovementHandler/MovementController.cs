using UnityEngine;

namespace D_Dev.MovementHandler
{
    public class MovementController : MonoBehaviour
    {
         #region Fields

         [SerializeReference] private BaseMovementHandler _movementHandler;

         protected bool _isStopped;

         #endregion

         #region Properties

         protected bool IsStopped => _isStopped;
         

         #endregion

         #region Monobehaviour
 
         protected virtual void Update()
         {
             if(_isStopped)
                 return;
             
             _movementHandler?.OnUpdate();
         }

         protected virtual void FixedUpdate()
         {
             if(_isStopped)
                 return;
             
             _movementHandler?.OnFixedUpdate();
         }

         #endregion
         
         #region Public
 
         public void SetDirection(Vector3 direction) => _movementHandler.Direction = direction;
         public void SetMaxVelocity(float maxVelocity) => _movementHandler.MaxVelocity = maxVelocity;
         public void SetAcceleration(float acceleration) => _movementHandler.Acceleration = acceleration;
         public void StopMovement()
         {
             _movementHandler.StopMovement();
             _isStopped = true;
         }
 
         public void ResumeMovement() => _isStopped = false;
         public float GetVelocity() => _movementHandler.GetVelocity();
         public bool IsMoving() => _movementHandler.IsMoving();
 
         #endregion
    }
}