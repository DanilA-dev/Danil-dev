using D_dev;
using D_Dev.CustomEventManager;
using UnityEngine;

namespace D_Dev.SceneLoader.Extensions.Actions
{
    [System.Serializable]
    public class SceneInfoAddAction : IAction
    {
        #region Fields

        [SerializeField] private SceneInfo _sceneInfo;

        #endregion

        #region Properties

        public bool IsFinished { get; set; }

        #endregion

        #region IAction

        public void Execute()
        {
            EventManager.Invoke(EventNameConstants.SceneAdd.ToString(), _sceneInfo.SceneName);
            IsFinished = true;
        }

        public void Undo() {}

        #endregion
    }
}