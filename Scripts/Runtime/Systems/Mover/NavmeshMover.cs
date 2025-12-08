using UnityEngine;
using UnityEngine.AI;

namespace D_Dev.Mover
{
    [System.Serializable]
    public class NavmeshMover : IMoverStrategy
    {
        #region Fields

        [SerializeField] private NavMeshAgent _owner;
        
        #endregion

        #region Public

        public Vector3 GetCurrentPosition() => _owner.transform.position;

        public void MoveTowards(Vector3 target, float speed, float deltaTime)
        {
            if (!Mathf.Approximately(_owner.speed, speed))
                _owner.speed = speed;
            
            _owner.SetDestination(target);
        }

        public bool IsAtPosition(Vector3 target, float tolerance)
        {
            return Vector3.Distance(_owner.transform.position, target) <= tolerance;
        }

        #endregion
    }
}
