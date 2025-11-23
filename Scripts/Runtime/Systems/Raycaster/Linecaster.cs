using D_Dev.PositionRotationConfig;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.Raycaster
{
    [System.Serializable]
    public class Linecaster
    {
        #region Fields

        [Title("Ray settings")]
        [SerializeField] private PositionConfig _origin;
        [SerializeField] private PositionConfig _direction;
        [SerializeField] private QueryTriggerInteraction _queryTriggerInteraction;
        [Title("Collider checker")]
        [SerializeField] private ColliderChecker.ColliderChecker _colliderChecker;
        [Title("Gizmos")]
        [SerializeField] private bool _drawGizmos;
        [SerializeField] private Color _debugColor = Color.yellow;
        
        #endregion
        
        #region Properties

        public PositionConfig Origin
        {
            get => _origin;
            set => _origin = value;
        }

        public PositionConfig Direction
        {
            get => _direction;
            set => _direction = value;
        }

        #endregion

        #region Public

        public bool IsIntersect()
        {
            return Physics.Linecast(_origin.GetPosition(), _direction.GetPosition(), out RaycastHit hit,  _colliderChecker.CheckLayer 
                       ? _colliderChecker.CheckLayerMask 
                       : ~0, _queryTriggerInteraction) 
                   && _colliderChecker.IsColliderPassed(hit.collider);
        }
        
        public bool IsIntersect(Vector3 origin, Vector3 direction)
        {
            return Physics.Linecast(origin, direction, out RaycastHit hit, _colliderChecker.CheckLayer 
                       ? _colliderChecker.CheckLayerMask 
                       : ~0, _queryTriggerInteraction) 
                   && _colliderChecker.IsColliderPassed(hit.collider);
        }

        #endregion
        
        #region Gizmos

        public void OnGizmos()
        {
            if(!_drawGizmos)
                return;
            
            if(_origin.Type == PositionConfig.PositionType.Transform 
               || _origin.Type == PositionConfig.PositionType.TransformDirection && _origin.Transform == null)
                return;
            
            if(_direction.Type == PositionConfig.PositionType.Transform 
               || _direction.Type == PositionConfig.PositionType.TransformDirection && _direction.Transform == null)
                return;
            
            var originPoint = _origin.GetPosition();
            var directionVector = _direction.GetPosition();
            Gizmos.color = _debugColor;
            Gizmos.DrawRay(originPoint, directionVector);
        }

        #endregion
    }
}