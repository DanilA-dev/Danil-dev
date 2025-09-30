using UnityEngine;
using D_Dev.TimerSystem;

namespace D_Dev.ScenarioSystem
{
    public class TimerExecutableNode : BaseScenarioExecutableNode
    {
        #region Fields

        [SerializeField] private float _duration = 1f;

        private CountdownTimer _timer;

        #endregion

        #region Public

        public override void Execute()
        {
            if (_timer.IsRunning)
                return;

            _timer.Start();
        }

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _timer = new CountdownTimer(_duration);
            _timer.OnTimerEnd += OnTimerEnd;
        }

        private void Update()
        {
            if (_timer.IsRunning)
            {
                _timer.Tick(Time.deltaTime);
            }
        }

        private void OnDestroy()
        {
            _timer.OnTimerEnd -= OnTimerEnd;
        }

        #endregion

        #region Private

        private void OnTimerEnd()
        {
            IsFinished = true;
        }

        #endregion
    }
}
