using D_Dev.ColliderEvents;
using D_Dev.PositionRotationConfig;
using D_Dev.Raycaster;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.TargetSensor
{
    [System.Serializable]
    public class TargetSensor
    {
        #region Fields

        [SerializeField] private TriggerColliderEvents _trigger;
        [SerializeField] private bool _checkObstacleLinecast;
        [ShowIf(nameof(_checkObstacleLinecast))]
        [SerializeField] private Linecaster _obstacleLinecaster;

        private PositionConfig _targetPoint;
        
        #endregion

        #region Properties

        public Collider Target { get; private set; }
        public bool IsTargetVisible { get; private set; }

        public TriggerColliderEvents Trigger
        {
            get => _trigger;
            set => _trigger = value;
        }

        public bool CheckObstacleLinecast
        {
            get => _checkObstacleLinecast;
            set => _checkObstacleLinecast = value;
        }

        public Linecaster ObstacleLinecaster
        {
            get => _obstacleLinecaster;
            set => _obstacleLinecaster = value;
        }

        #endregion
        
        #region Public

        public void Init()
        {
            if(_trigger == null)
                return;
            
            _targetPoint = new();
            _trigger.OnEnter.AddListener(OnTargetEnter);
            _trigger.OnExit.AddListener(OnTargetExit);
        }

        public void Dispose()
        {
            if(_trigger == null)
                return;
            
            _trigger.OnEnter.RemoveListener(OnTargetEnter);
            _trigger.OnExit.RemoveListener(OnTargetExit);
        }
        
        public bool IsTargetFound()
        {
            if (!_checkObstacleLinecast)
                return Target != null;
            else
                return Target != null && _obstacleLinecaster.IsIntersect();
        }
        
        public bool IsTargetFound(out Collider target)
        {
            target = Target;
            if (!_checkObstacleLinecast)
                return target != null;
            else
                return target != null && _obstacleLinecaster.IsIntersect();
        }

        #endregion
        
        #region Listeners

        private void OnTargetEnter(Collider target)
        {
            Target = target;
            _targetPoint.Transform = Target.transform;
            _targetPoint.Type = PositionConfig.PositionType.TransformDirection;
            _obstacleLinecaster.Direction = _targetPoint;
        }

        private void OnTargetExit(Collider target) => Target = null;

        #endregion

    }
}