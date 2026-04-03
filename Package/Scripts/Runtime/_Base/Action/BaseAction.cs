#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.Base
{
    [System.Serializable]
    public abstract class BaseAction
    {
        #region Fields

        [SerializeField]
#if ODIN_INSPECTOR
        [PropertyOrder(-1)]
#endif
        private string _actionName;

#if ODIN_INSPECTOR
        [FoldoutGroup("Events"), PropertyOrder(100)]
#endif
        public UnityEvent OnUndo;

        #endregion

        #region Properties

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
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