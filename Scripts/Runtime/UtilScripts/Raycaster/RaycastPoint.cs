using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.Raycaster
{
    [System.Serializable]
    public class RaycastPoint
    {
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

        public RaycastPointType PointType
        {
            get => _raycastPointType;
            set => _raycastPointType = value;
        }
        public Vector3 RaycastVectorPoint
        {
            get => _raycastVectorPoint;
            set => _raycastVectorPoint = value;
        }
        public Transform RaycastTransformPoint
        {
            get => _raycastTransformPoint;
            set => _raycastTransformPoint = value;
        }

        #endregion

        #region Public

        public Vector3 GetPoint()
        {
            return _raycastPointType switch
            {
                RaycastPointType.Vector => _raycastVectorPoint,
                RaycastPointType.Transform => _localTransformDirection switch
                {
                    LocalTransformDirection.Self => _raycastTransformPoint.position,
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
}
