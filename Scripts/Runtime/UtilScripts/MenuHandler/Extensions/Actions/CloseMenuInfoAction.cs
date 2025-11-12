using D_Dev.CustomEventManager;
using D_Dev.MenuHandler;
using UnityEngine;

namespace D_dev.Scripts.MenuHandler.Extensions.Actions
{
    [System.Serializable]
    public class CloseMenuInfoAction : IAction
    {
        #region Fields

        [SerializeField] private MenuInfo _menuInfo;

        #endregion
        
        #region Properties
        public bool IsFinished { get; set; }
        
        #endregion

        #region IAction

        public void Execute()
        {
            EventManager.Invoke(EventNameConstants.MenuClose.ToString(), _menuInfo);
            IsFinished = true;
        }

        public void Undo()
        {
            EventManager.Invoke(EventNameConstants.MenuOpen.ToString(), _menuInfo);
            IsFinished = false;
        }

        #endregion
    }
}