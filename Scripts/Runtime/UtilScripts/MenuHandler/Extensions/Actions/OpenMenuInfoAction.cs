using D_Dev.CustomEventManager;
using D_Dev.MenuHandler;
using UnityEngine;

namespace D_dev.Scripts.MenuHandler.Extensions.Actions
{
    [System.Serializable]
    public class OpenMenuInfoAction : IAction
    {
        #region Fields

        [SerializeField] private MenuInfo _menuInfo;

        #endregion
        
        #region Properties

        public bool IsFinished
        {
            get
            {
                if(D_Dev.MenuHandler.MenuHandler.Instance == null)
                    return false;
                return D_Dev.MenuHandler.MenuHandler.IsMenuOpen(_menuInfo);
            }
        }
        
        #endregion

        #region IAction

        public void Execute()
        {
            EventManager.Invoke(EventNameConstants.MenuOpen.ToString(), _menuInfo);
        }

        public void Undo()
        {
            EventManager.Invoke(EventNameConstants.MenuClose.ToString(), _menuInfo);
        }

        #endregion
    }
}