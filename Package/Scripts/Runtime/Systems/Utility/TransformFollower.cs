using System;
using D_Dev.PolymorphicValueSystem;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace D_Dev.Utility
{
    public class TransformFollower : MonoBehaviour
    {
        #region Enum

        [Flags]
        public enum AxisUpdate
        {
            X = 1 << 0,
            Y = 1 << 1,
            Z = 1 << 2,
            All = X | Y | Z
        }

        #endregion
        
        #region Fields

        [Title("Transforms")]
        [SerializeReference] private PolymorphicValue<Transform> _follower;
        [SerializeReference] private PolymorphicValue<Transform> _objectToFollow;
        [Title("Settings")]
        [SerializeField] private float _tickInterval = 0.05f;
        [SerializeField] private bool _updatePosition = true;
        [SerializeField] private bool _updateRotation = true;

        [ShowIf(nameof(_updatePosition))]
        [FoldoutGroup("Update Position Settings")] 
        [SerializeField] private float _positionSpeed = 5f;
        [FoldoutGroup("Update Position Settings")] 
        [ShowIf(nameof(_updatePosition))]
        [SerializeField] private AxisUpdate _posAxisUpdate = AxisUpdate.All;
        
        [ShowIf(nameof(_updateRotation))]
        [FoldoutGroup("Update Rotation Settings")] 
        [SerializeField] private float _rotationSpeed = 5f;
        [FoldoutGroup("Update Rotation Settings")] 
        [ShowIf(nameof(_updateRotation))]
        [SerializeField] private AxisUpdate _rotAxisUpdate = AxisUpdate.All;
        
        private CompositeDisposable _disposables;
        private RotationHandler _rotationHandler;
        private Vector3 _lastTargetPosition;

        #endregion

        #region Monobehaviour

        private void Start()
        {
            _disposables = new CompositeDisposable();
            _rotationHandler = new RotationHandler();

            if (!IsFollowerNull())
                _rotationHandler.Initialize(_follower.Value, _rotationSpeed);

            SetupObservables();
        }

        private void OnDestroy()
        {
            _disposables?.Dispose();
        }

        #endregion

        #region Private

        private void SetupObservables()
        {
            Observable.Interval(TimeSpan.FromSeconds(_tickInterval))
                .Subscribe(_ =>
                {
                    if (!IsObjectToFollowNull())
                        _lastTargetPosition = _objectToFollow.Value.position;
                })
                .AddTo(_disposables);

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    UpdatePosition();
                    UpdateRotation();
                })
                .AddTo(_disposables);
        }

        private void UpdatePosition()
        {
            if (!_updatePosition || IsFollowerNull())
                return;

            Vector3 currentPos = _follower.Value.position;
            Vector3 targetPos = FilterAxis(_lastTargetPosition, currentPos, _posAxisUpdate);
            _follower.Value.position = Vector3.Lerp(currentPos, targetPos, _positionSpeed * Time.deltaTime);
        }

        private void UpdateRotation()
        {
            if (!_updateRotation || IsFollowerNull() || IsObjectToFollowNull())
                return;

            Vector3 direction = _objectToFollow.Value.position - _follower.Value.position;
            Vector3 filteredDirection = FilterAxis(direction, Vector3.zero, _rotAxisUpdate);
            _rotationHandler.RotateTowards(filteredDirection, _rotationSpeed);
        }

        private Vector3 FilterAxis(Vector3 target, Vector3 current, AxisUpdate axisUpdate)
        {
            Vector3 result = current;
            if ((axisUpdate & AxisUpdate.X) != 0) result.x = target.x;
            if ((axisUpdate & AxisUpdate.Y) != 0) result.y = target.y;
            if ((axisUpdate & AxisUpdate.Z) != 0) result.z = target.z;
            return result;
        }

        private bool IsFollowerNull() => _follower == null || _follower.Value == null;
        private bool IsObjectToFollowNull() => _objectToFollow == null || _objectToFollow.Value == null;

        #endregion
    }
}
