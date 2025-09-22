using UnityEngine;

namespace D_dev.Scripts.ScenarioSystem
{
    public abstract class BaseScenarioExecutableNode : MonoBehaviour
    {
        #region Properties

        public bool IsFinished { get; protected set; }

        #endregion

        #region Public

        public void ForceFinish() => IsFinished = true;

        #endregion

        #region Abstract

        public virtual void Execute() {}

        #endregion
    }
}