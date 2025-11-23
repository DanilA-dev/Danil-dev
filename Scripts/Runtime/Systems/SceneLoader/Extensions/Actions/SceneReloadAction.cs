using D_dev;
using D_Dev.CustomEventManager;

namespace D_Dev.SceneLoader.Extensions.Actions
{
    [System.Serializable]
    public class SceneReloadAction : BaseAction
    {
        #region IAction

        public override void Execute()
        {
            EventManager.Invoke(EventNameConstants.SceneReload.ToString());
            IsFinished = true;
        }

        #endregion
    }
}