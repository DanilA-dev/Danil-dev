using System;
using D_Dev.Base;
using D_Dev.StateMachine;
using UnityEngine;

namespace D_Dev.StateMachineBehaviour
{
    public abstract class BaseModularState<TStateEnum> : MonoBehaviour, IState where TStateEnum : Enum
    {
        #region Classes

        [Serializable]
        public class TransitionData
        {
            #region Fields

            [SerializeField] private TStateEnum[] _fromStates;
            [SerializeReference] private ICondition[] _conditions;

            #endregion

            #region Properties

            public TStateEnum[] FromStates => _fromStates;

            public ICondition[] Conditions => _conditions;

            #endregion
        }

        #endregion

        #region Fields

        [SerializeField] private TransitionData[] _transitions;

        #endregion
        
        #region Properties

        public float ExitTime { get; }
        public abstract TStateEnum StateName { get; }

        public TransitionData[] Transitions => _transitions;

        #endregion

        #region IState

        public virtual void OnEnter(){}

        public virtual void OnUpdate(){}

        public virtual void OnFixedUpdate(){}

        public virtual void OnExit(){}


        #endregion
    }
}