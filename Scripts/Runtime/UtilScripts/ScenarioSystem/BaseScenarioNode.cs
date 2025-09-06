using UnityEngine;

namespace D_dev.Scripts.ScenarioSystem
{
    public abstract class BaseScenarioNode : MonoBehaviour
    {
        public bool IsFinished { get; protected set; }
        public bool IsExecuting { get; protected set; }
        
        public abstract void Execute();
    }
}