using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.UtilScripts.Raycaster
{
    [System.Serializable]
    public class Linecaster
    {
        #region Fields

        [Title("Ray settings")]
        [SerializeField] private RaycastPoint _origin;
        [SerializeField] private RaycastPoint _direction;
        [Title("Collider checker")]
        [SerializeField] private ColliderChecker.ColliderChecker _colliderChecker;
        
        #endregion
        
        #region Properties

        public RaycastPoint Origin
        {
            get => _origin;
            set => _origin = value;
        }

        public RaycastPoint Direction
        {
            get => _direction;
            set => _direction = value;
        }

        #endregion

        #region Public

        public bool IsIntersect()
        {
            return Physics.Linecast(_origin.GetPoint(), _direction.GetPoint(), out RaycastHit hit) 
                   && _colliderChecker.IsColliderPassed(hit.collider);
        }
        
        public bool IsIntersect(Vector3 origin, Vector3 direction)
        {
            return Physics.Linecast(origin, direction, out RaycastHit hit) 
                   && _colliderChecker.IsColliderPassed(hit.collider);
        }

        #endregion
    }
}