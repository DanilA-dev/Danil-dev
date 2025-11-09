using D_dev;
using D_Dev.CustomEventManager;

namespace D_Dev.SceneLoader.Extensions.Actions
{
    [System.Serializable]
    public class SceneReloadAction :IAction
    {
        #region Properties

        public bool IsFinished { get; set; }

        #endregion

        #region IAction

        public void Execute()
        {
            EventManager.Invoke(EventNameConstants.SceneReload.ToString());
            IsFinished = true;
        }

        public void Undo() {}

        #endregion
    }
}