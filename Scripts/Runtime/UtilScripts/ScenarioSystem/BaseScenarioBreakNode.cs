using UnityEngine;

namespace D_Dev.ScenarioSystem
{
    public abstract class BaseScenarioBreakNode : MonoBehaviour
    {
        #region Fields

        [SerializeField] protected bool _isActive = true;
        [SerializeField] protected bool _breakOnce = true;
        
        private bool _hasTriggered;

        #endregion

        #region Properties

        public bool IsActive => _isActive;
        public bool HasTriggered => _hasTriggered;

        #endregion

        #region Public

        public bool ShouldBreakScenario()
        {
            if (!_isActive || (_breakOnce && _hasTriggered))
                return false;
                
            bool shouldBreak = CheckBreakCondition();
            
            if (shouldBreak)
            {
                _hasTriggered = true;
                OnScenarioBreak();
            }
            
            return shouldBreak;
        }
        public void Reset() => _hasTriggered = false;
        public void SetActive(bool active) => _isActive = active;

        #endregion

        #region Virtual/Abstract

        protected abstract bool CheckBreakCondition();
        
        protected virtual void OnScenarioBreak() {}

        #endregion
    }
}
