using D_Dev.TimerSystem;
using UnityEngine;

namespace D_dev.Actions
{
    [System.Serializable]
    public class TimerAction : BaseAction
    {
        #region Fields

        [SerializeField] private float _delay;
        
        private CountdownTimer _timer;

        #endregion

        #region IAction
        
        public override void Execute()
        {
            if(_timer == null)
                _timer = new CountdownTimer(_delay);

            if (!_timer.IsRunning)
            {
                _timer.Start();
                _timer.OnTimerEnd += OnTimerEnd;
            }
            else
                _timer.Tick(Time.deltaTime);
        }

        #endregion

        #region Listeners

        private void OnTimerEnd()
        {
            _timer.OnTimerEnd -= OnTimerEnd;
            IsFinished = true;
            _timer.Reset();
            _timer.Stop();
        }

        #endregion
    }
}
