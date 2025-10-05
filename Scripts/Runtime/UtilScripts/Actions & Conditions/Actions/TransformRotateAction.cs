using UnityEngine;

namespace D_dev.Actions
{
    [System.Serializable]
    public class TransformRotateAction : IAction
    {
        public enum RotationAxis
        {
            All,
            X,
            Y,
            Z
        }

        #region Fields

        [SerializeField] private Transform _transformToRotate;
        [Space]
        [SerializeField] private TargetInfo.TargetInfo _target;
        [SerializeField] private RotationAxis _rotationAxis = RotationAxis.All;
        [SerializeField] private float _rotationSpeed = 180f;
        [SerializeField] private float _angleThreshold = 1f;

        #endregion

        #region IAction

        public void Execute()
        {
            if (_transformToRotate == null)
                return;

            Vector3 targetPosition = _target.GetTargetPosition();
            Vector3 direction = targetPosition - _transformToRotate.position;

            if (direction == Vector3.zero)
                return;

            Quaternion targetRotation = GetTargetRotation(direction);
            _transformToRotate.rotation = Quaternion.RotateTowards(_transformToRotate.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        private Quaternion GetTargetRotation(Vector3 direction)
        {
            switch (_rotationAxis)
            {
                case RotationAxis.All:
                    return Quaternion.LookRotation(direction);
                case RotationAxis.X:
                    float angleX = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
                    return Quaternion.Euler(angleX, 0, 0);
                case RotationAxis.Y:
                    float angleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    return Quaternion.Euler(0, angleY, 0);
                case RotationAxis.Z:
                    float angleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    return Quaternion.Euler(0, 0, angleZ);
                default:
                    return Quaternion.LookRotation(direction);
            }
        }

        public bool IsFinished
        {
            get
            {
                if (_transformToRotate == null)
                    return true;

                Vector3 targetPosition = _target.GetTargetPosition();
                Vector3 direction = targetPosition - _transformToRotate.position;

                if (direction == Vector3.zero)
                    return true;

                Quaternion targetRotation = GetTargetRotation(direction);
                return Quaternion.Angle(_transformToRotate.rotation, targetRotation) < _angleThreshold;
            }
        }

        #endregion
    }
}
