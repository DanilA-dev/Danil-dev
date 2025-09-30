using UnityEngine;

namespace D_Dev.ScenarioSystem
{
    public class ToggleObjectExecutableNode : BaseScenarioExecutableNode
    {
        #region Fields
        
        [SerializeField] private GameObject _targetObject;
        [SerializeField] private bool _enable = true;
        
        #endregion

        #region Overrides
        public override void Execute()
        {
            if (_targetObject != null)
                _targetObject.SetActive(_enable);
            
            IsFinished = true;
        }
        #endregion
    }
}
