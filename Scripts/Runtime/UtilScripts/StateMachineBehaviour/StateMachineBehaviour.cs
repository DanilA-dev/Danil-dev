using System;
using Cysharp.Threading.Tasks;
using D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.Scripts.Runtime.UtilScripts.StateMachineBehaviour
{
    public abstract class StateMachineBehaviour<TStateEnum> : MonoBehaviour where TStateEnum : Enum
    {
        #region Fields

        [SerializeField, ReadOnly] protected TStateEnum _currentState;
        [Space]
        [SerializeField] protected TStateEnum _startState;
        [FoldoutGroup("Events")]
        public UnityEvent<TStateEnum> OnStateEnter;
        [FoldoutGroup("Events")]
        public UnityEvent<TStateEnum> OnStateExit;
        
        protected StateMachine<TStateEnum> _stateMachine;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _stateMachine = new StateMachine<TStateEnum>();
            _stateMachine.OnStateEnter += state =>
            {
                OnStateEnter?.Invoke(state);
                _currentState = state;
            };
            _stateMachine.OnStateExit += state => OnStateExit?.Invoke(state);
            InitStates();
        }

        private void OnDestroy()
        {
            _stateMachine.OnStateEnter -= state =>
            {
                OnStateEnter?.Invoke(state);
                _currentState = state;
            };
            _stateMachine.OnStateExit -= state => OnStateExit?.Invoke(state);
        }

        private void Update()
        {
            _stateMachine?.OnUpdate();
            OnUpdate();
        }

        private void FixedUpdate()
        {
            _stateMachine?.OnFixedUpdate();
            OnFixedUpdate();
        }

        #endregion

        #region Virtual/Abstract

        protected abstract void InitStates();
        protected virtual void OnUpdate() {}
        protected virtual void OnFixedUpdate() {}

        #endregion

        #region Protected

        protected void AddState(TStateEnum stateName, IState state) => _stateMachine?.AddState(stateName, state);
        protected void AddTransition(TStateEnum fromState, TStateEnum toState, IStateCondition condition) 
            => _stateMachine?.AddTransition(fromState, toState, condition);
        protected void ChangeState(TStateEnum stateName) => _stateMachine.ChangeState(stateName);
        
        #endregion
        
    }
}