using System.Linq;
using UnityEngine;

namespace D_dev.ActionGroup
{
    [System.Serializable]
    public class ActionGroup
    {
        #region Fields

        [SerializeField] private bool _executeInParallel = true;
        [Space]
        [SerializeReference] private IAction[] _actions;
        
        #endregion

        #region Properties

        public bool ExecuteInParallel => _executeInParallel;
        public IAction[] Actions => _actions;
        public bool IsCompleted => Actions.All(a => a == null || a.IsFinished);
        
        #endregion
        
        #region Public
       
        public void Execute()
        {
            if (Actions == null || Actions.Length == 0)
                return;
            
            if (ExecuteInParallel)
            {
                foreach (var action in Actions)
                {
                    if (action != null && !action.IsFinished)
                        action.Execute();
                }
            }
            else
            {
                foreach (var action in Actions)
                {
                    if(action == null)
                        continue;
                    
                    if (!action.IsFinished)
                    {
                        action.Execute();
                        break;
                    }
                }
            }
        }
       
       #endregion
    }
}
