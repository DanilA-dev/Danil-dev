using UnityEngine;

namespace CustomFSM.Preicate
{
    public class DelayPredicate : IPredicate
    {
        private CountdownTimer _timer;

        public bool CanBeUpdated { get; set; } = true;
        public DelayPredicate(float time)
        {
            _timer = new CountdownTimer(time);
            _timer.Start();
        }


        public bool Evaluate()
        {
            if(!_timer.IsRunning && _timer.RemainingTime <= 0)
                _timer.Start();
            
            _timer.Tick(Time.deltaTime);
            return CanBeUpdated && !_timer.IsRunning;
        }
    }
}