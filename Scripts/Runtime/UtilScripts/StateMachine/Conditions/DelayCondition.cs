using UnityEngine;

namespace D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine
{
    public class DelayCondition : IStateCondition
    {
        private float _maxTime;
        private float _currentTime;
        
        public DelayCondition(float maxTime)
        {
            _currentTime = maxTime;
        }
        
        public bool IsMatched()
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0)
            {
                _currentTime = _maxTime;
                return true;
            }
            return false;
        }
    }
}