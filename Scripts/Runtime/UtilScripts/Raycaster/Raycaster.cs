using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.UtilScripts.Raycaster
{
    public enum RaycastPointType
    {
        Vector,
        Transform
    }
            
    public enum LocalTransformDirection
    {
        Self = 0,
        Up = 1,
        Down = 2,
        Right = 3,
        Left = 4,
        Forward = 5,
        Back = 7
    }

    [System.Serializable]
    public class Raycaster
    {
        #region Fields

        [Title("Ray settings")]
        [SerializeField] private float _distance;
        [SerializeField] private int _collidersBuffer;
        [SerializeField] private RaycastPoint _origin;
        [SerializeField] private RaycastPoint _direction;
        [Title("Collider checker")]
        [SerializeField] private ColliderChecker.ColliderChecker _colliderChecker;
        [Space]
        [Title("Gizmos")] 
        [SerializeField] private Color _debugColor;

        private Ray _ray = new();
        private RaycastHit[] _hits;
        
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

        public float Distance
        {
            get => _distance;
            set => _distance = value;
        }

        public int CollidersBuffer
        {
            get => _collidersBuffer;
            set => _collidersBuffer = value;
        }

        #endregion

        #region Public

        public bool IsHit()
        {
            _hits ??= new RaycastHit[_collidersBuffer];
            _ray.origin = _origin.GetPoint();
            _ray.direction = _direction.GetPoint();
            var hitsAmount = Physics.RaycastNonAlloc(_ray, _hits, _distance);
            Debug.DrawRay(_ray.origin, _ray.direction, _debugColor);
            if (hitsAmount > 0)
            {
                for (var i = 0; i < _hits.Length; i++)
                {
                    if (_hits[i].collider != null
                        && _colliderChecker.IsColliderPassed(_hits[i].collider))
                        return true;
                }
            }
            return false;
        }
        
        public bool IsHit(Vector3 origin, Vector3 direction)
        {
            _hits ??= new RaycastHit[_collidersBuffer];
            _ray.origin = origin;
            _ray.direction = direction;
            var hitsAmount = Physics.RaycastNonAlloc(_ray, _hits, _distance);
            Debug.DrawRay(_ray.origin, _ray.direction, _debugColor);
            if (hitsAmount > 0)
            {
                for (var i = 0; i < _hits.Length; i++)
                {
                    if (_hits[i].collider != null
                        && _colliderChecker.IsColliderPassed(_hits[i].collider))
                        return true;
                }
            }
            return false;
        }
        
        public bool IsHit(Vector3 origin, Vector3 direction, out Collider collider)
        {
            _hits ??= new RaycastHit[_collidersBuffer];
            _ray.origin = origin;
            _ray.direction = direction;
            var hitsAmount = Physics.RaycastNonAlloc(_ray, _hits, _distance);
            Debug.DrawRay(_ray.origin, _ray.direction, _debugColor);
            if (hitsAmount > 0)
            {
                for (var i = 0; i < _hits.Length; i++)
                {
                    if (_hits[i].collider != null
                        && _colliderChecker.IsColliderPassed(_hits[i].collider))
                    {
                        collider = _hits[i].collider;
                        return true;
                    }
                }
            }
            collider = null;
            return false;
        }

        public bool IsHit(out Collider collider)
        {
            _hits ??= new RaycastHit[_collidersBuffer];
            _ray.origin = _origin.GetPoint();
            _ray.direction = _direction.GetPoint();
            var hitsAmount = Physics.RaycastNonAlloc(_ray, _hits, _distance);
            Debug.DrawRay(_ray.origin, _ray.direction, _debugColor);
            if (hitsAmount > 0)
            {
                for (var i = 0; i < _hits.Length; i++)
                {
                    if (_hits[i].collider != null
                        && _colliderChecker.IsColliderPassed(_hits[i].collider))
                    {
                        collider = _hits[i].collider;
                        return true;
                    }
                }
            }
            collider = null;
            return false;
        }

        #endregion

        #region Gizmos

        public void OnGizmos()
        {
            if(_origin.PointType == RaycastPointType.Transform &&
               _origin.RaycastTransformPoint == null ||
               _direction.PointType == RaycastPointType.Transform &&
               _direction.RaycastTransformPoint == null)
                return;
            
            Gizmos.color = _debugColor;
            Gizmos.DrawRay(_origin.GetPoint(), (_direction.GetPoint() * _distance));
        }

        #endregion
    }
}