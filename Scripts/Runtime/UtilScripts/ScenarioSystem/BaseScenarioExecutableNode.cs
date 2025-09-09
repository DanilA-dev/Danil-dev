using UnityEngine;

namespace D_dev.Scripts.ScenarioSystem
{
    public abstract class BaseScenarioExecutableNode : MonoBehaviour
    {
        public bool IsFinished { get; protected set; }
        
        public abstract void Execute();
        public void ForceFinish() => IsFinished = true;
    }
}