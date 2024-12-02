using Project.Scripts.ColliderChecker;
using UnityEngine;

public class TestColliderChecker : MonoBehaviour
{
   [SerializeField] private ColliderChecker _colliderChecker;

   private void OnTriggerEnter(Collider other)
   {
      if(_colliderChecker.IsColliderPassed(other))
         Debug.Log($"Collider {other.name} passed");
   }

   private void OnCollisionEnter(Collision other)
   {
      if(_colliderChecker.IsColliderPassed(other.collider))
         Debug.Log($"Collider {other.collider.name} passed");
   }
}
