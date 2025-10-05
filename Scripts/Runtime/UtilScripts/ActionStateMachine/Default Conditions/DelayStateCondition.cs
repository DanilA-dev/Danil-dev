using UnityEngine;

namespace D_dev.UltimateStateController.Default_Conditions
{
    [System.Serializable]
    public class DelayStateCondition : BaseStateCondition
    {
        #region Fields

        [SerializeField] private float _delay;
        
        private bool _isStarted;
        private float _startTime;

        #endregion

        #region Overrides

        public override bool IsConditionMet()
        {
            if (!_isStarted)
            {
                _isStarted = true;
                _startTime = Time.time;
                return false;
            }

            return Time.time - _startTime >= _delay;
        }

        #endregion
    }
}
