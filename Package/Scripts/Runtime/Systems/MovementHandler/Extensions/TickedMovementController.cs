using UnityEngine;
using D_Dev.UpdateManagerSystem;

namespace D_Dev.MovementHandler
{
    public class TickedMovementController : MonoBehaviour, ITickable, IFixedTickable
    {
        #region Fields

        [SerializeReference] private BaseMovementHandler _movementHandler;

        [SerializeField] private int _priority = 0;

        [SerializeField, Min(0f)] private float _updateInterval = 0f;

        protected bool _isStopped;

        private float _updateTimer;

        #endregion

        #region Properties

        protected bool IsStopped => _isStopped;

        public float UpdateInterval
        {
            get => _updateInterval;
            set => _updateInterval = Mathf.Max(0f, value);
        }

        #endregion

        #region Monobehaviour

        private void Awake() => _movementHandler?.OnAwake();

        private void Start()
        {
            _movementHandler?.OnStart();
            UpdateManager.AddWithPriority(this, _priority);
            FixedUpdateManager.AddWithPriority(this, _priority);
        }

        private void OnDestroy()
        {
            UpdateManager.Remove(this);
            FixedUpdateManager.Remove(this);
        }

        #endregion

        #region ITickable

        public void Tick()
        {
            if (_isStopped)
                return;

            if (_updateInterval > 0f)
            {
                _updateTimer += Time.deltaTime;
                if (_updateTimer < _updateInterval)
                    return;
                _updateTimer = 0f;
            }

            _movementHandler?.OnUpdate();
        }

        #endregion

        #region IFixedTickable

        public void FixedTick()
        {
            if (_isStopped)
                return;

            _movementHandler?.OnFixedUpdate();
        }

        #endregion

        #region Public

        public void SetDirection(Vector3 direction) => _movementHandler.Direction = direction;
        public void SetMaxVelocity(float maxVelocity) => _movementHandler.MaxVelocity = maxVelocity;
        public void SetAcceleration(float acceleration) => _movementHandler.Acceleration = acceleration;

        public void StopMovement()
        {
            _movementHandler.StopMovement();
            _isStopped = true;
        }

        public void ResumeMovement() => _isStopped = false;
        public float GetVelocity() => _movementHandler.GetVelocity();
        public bool IsMoving() => _movementHandler.IsMoving();

        public int GetPriority() => _priority;
        public void SetPriority(int priority) => _priority = priority;

        #endregion
    }
}
