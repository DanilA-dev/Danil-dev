using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace D_Dev.Base
{
    [System.Serializable]
    public abstract class BaseAction
    {
        #region Fields

        [FoldoutGroup("Events"), PropertyOrder(100)] 
        public UnityEvent OnUndo;

        #endregion

        #region Properties

        [ShowInInspector, ReadOnly]
        public bool IsFinished { get; set; }

        #endregion

        #region Abstract/Virtual

        public abstract void Execute();

        public virtual void Undo()
        {
            IsFinished = false;
            OnUndo?.Invoke();
        }

        #endregion
    }
}