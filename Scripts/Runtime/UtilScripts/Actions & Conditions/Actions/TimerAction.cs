using D_Dev.TimerSystem;
using UnityEngine;

namespace D_dev.Actions
{
    [System.Serializable]
    public class TimerAction : IAction
    {
        #region Fields

        [SerializeField] private float _delay;
        
        private CountdownTimer _timer;
        private bool _isFinished;

        #endregion

        #region IAction
        
        public bool IsFinished => _isFinished;
        

        public void Execute()
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

        public void Undo()
        {
            _isFinished = false;
        }


        #endregion

        #region Listeners

        private void OnTimerEnd()
        {
            _timer.OnTimerEnd -= OnTimerEnd;
            _isFinished = true;
            _timer.Reset();
            _timer.Stop();
        }

        #endregion
    }
}
