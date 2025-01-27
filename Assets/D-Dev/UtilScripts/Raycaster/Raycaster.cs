using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.UtilScripts.Raycaster
{
    [System.Serializable]
    public class Raycaster
    {
        #region Classes

         [System.Serializable]
        public class RaycastPoint
        {
            #region Enums

            public enum RaycastPointType
            {
                Vector,
                Transform
            }
            
            public enum LocalTransformDirection
            {
                None = 0,
                Up = 1,
                Down = 2,
                Right = 3,
                Left = 4,
                Forward = 5,
                Back = 7
            }

            #endregion

            #region Fields

            [SerializeField] private RaycastPointType _raycastPointType;
            [ShowIf(nameof(_raycastPointType), RaycastPointType.Vector)] 
            [SerializeField] private Vector3 _raycastVectorPoint;
            [ShowIf(nameof(_raycastPointType), RaycastPointType.Transform)] 
            [SerializeField] private Transform _raycastTransformPoint;
            [ShowIf(nameof(_raycastPointType), RaycastPointType.Transform)] 
            [SerializeField] private LocalTransformDirection _localTransformDirection;

            #endregion

            #region Properties

            public Transform RaycastTransformPoint => _raycastTransformPoint;
            public RaycastPointType PointType => _raycastPointType;

            #endregion

            #region Public

            public Vector3 GetPoint()
            {
                return _raycastPointType switch
                {
                    RaycastPointType.Vector => _raycastVectorPoint,
                    RaycastPointType.Transform => _localTransformDirection switch
                    {
                        LocalTransformDirection.None => _raycastTransformPoint.position,
                        LocalTransformDirection.Up => _raycastTransformPoint.up,
                        LocalTransformDirection.Down => -_raycastTransformPoint.up,
                        LocalTransformDirection.Right => _raycastTransformPoint.right,
                        LocalTransformDirection.Left => -_raycastTransformPoint.right,
                        LocalTransformDirection.Forward => _raycastTransformPoint.forward,
                        LocalTransformDirection.Back => -_raycastTransformPoint.forward,
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            #endregion
        }

        #endregion

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
            var results = new RaycastHit[_collidersBuffer];
            var ray = new Ray(_origin.GetPoint(), _direction.GetPoint());
            var hitsAmount = Physics.RaycastNonAlloc(ray, results, _distance);
            Debug.DrawRay(ray.origin, ray.direction, _debugColor);
            if (hitsAmount > 0)
            {
                for (var i = 0; i < results.Length; i++)
                {
                    if (results[i].collider != null
                        && _colliderChecker.IsColliderPassed(results[i].collider))
                        return true;
                }
            }
            return false;
        }

        public bool IsHit(out Collider collider)
        {
            var results = new RaycastHit[_collidersBuffer];
            var ray = new Ray(_origin.GetPoint(), _direction.GetPoint());
            var hitsAmount = Physics.RaycastNonAlloc(ray, results, _distance);
            Debug.DrawRay(ray.origin, ray.direction, _debugColor);
            if (hitsAmount > 0)
            {
                for (var i = 0; i < results.Length; i++)
                {
                    if (results[i].collider != null
                        && _colliderChecker.IsColliderPassed(results[i].collider))
                    {
                        collider = results[i].collider;
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
            if(_origin.PointType == RaycastPoint.RaycastPointType.Transform &&
               _origin.RaycastTransformPoint == null ||
               _direction.PointType == RaycastPoint.RaycastPointType.Transform &&
               _direction.RaycastTransformPoint == null)
                return;
            
            Gizmos.color = _debugColor;
            Gizmos.DrawRay(_origin.GetPoint(), (_direction.GetPoint() * _distance));
        }

        #endregion
    }
}