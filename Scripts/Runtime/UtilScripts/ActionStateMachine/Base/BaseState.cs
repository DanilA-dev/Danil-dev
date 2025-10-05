using System.Threading;
using D_Dev.ScriptableVaiables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_dev.UltimateStateController
{
    [System.Serializable]
    public abstract class BaseState
    {
        #region Fields

        [SerializeField] protected StringScriptableVariable _stateID;
        [SerializeField] protected bool _hasExitTime;
        [ShowIf("_hasExitTime")]
        [SerializeField] protected float _exitTime;
        [PropertySpace(10)]
        [PropertyOrder(1)]
        [SerializeField] protected StateTransition[] _transitions;
        [Space]
        [FoldoutGroup("Events"), PropertyOrder(2)]
        public UnityEvent OnEnterEvent;
        [FoldoutGroup("Events"), PropertyOrder(2)]
        public UnityEvent OnExitEvent;

        #endregion

        #region Properties

        public bool HasExitTime => _hasExitTime;
        public float ExitTime => _exitTime;
        public StringScriptableVariable StateID => _stateID;
        public StateTransition[] Transitions => _transitions;

        #endregion
        
        #region Public

        public void Enter()
        {
            OnEnter();
            OnEnterEvent?.Invoke();
        }
        
        public void Update()
        {
            OnUpdate();
        }

        public void FixedUpdate()
        {
            OnFixedUpdate();
        }
        
        public void Exit()
        {
            OnExit();
            OnExitEvent?.Invoke();
        }
        
        #endregion

        #region Virtual

        public virtual void OnEnter() {}
        public virtual void OnUpdate() {}
        public virtual void OnFixedUpdate() {}
        public virtual void OnExit() {}

        #endregion
    }
}
