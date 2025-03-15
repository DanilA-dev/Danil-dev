using System;
using System.Linq;
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
        [SerializeField] protected bool _debugStateChange;
        [Space]
        [SerializeField] protected TStateEnum _startState;
        [FoldoutGroup("Events")]
        [SerializeField] protected StateEvent<TStateEnum>[] _stateEvents;
        [FoldoutGroup("Events")]
        public UnityEvent<TStateEnum> OnAnyStateEnter;
        [FoldoutGroup("Events")]
        public UnityEvent<TStateEnum> OnAnyStateExit;
        
        protected StateMachine<TStateEnum> _stateMachine;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _stateMachine = new StateMachine<TStateEnum>();
            _stateMachine.OnStateEnter += state =>
            {
                _currentState = state;
                OnAnyStateEnter?.Invoke(state);
                InvokeStateEnterEvent(state);
                StateChangedDebug(state);
            };
            _stateMachine.OnStateExit += InvokeStateExitEvent;
            InitStates();
        }

        private void OnDestroy()
        {
            _stateMachine.OnStateEnter -= state =>
            {
                _currentState = state;
                OnAnyStateExit?.Invoke(state);
                InvokeStateEnterEvent(state);
            };
            _stateMachine.OnStateExit -= InvokeStateExitEvent;
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
        protected void AddTransition(TStateEnum[] fromStates, TStateEnum toState, IStateCondition condition)
        {
            foreach (var fromState in fromStates)
                _stateMachine?.AddTransition(fromState, toState, condition);
        }

        protected void ChangeState(TStateEnum stateName) => _stateMachine.ChangeState(stateName);
        
        #endregion

        #region Private

        private void InvokeStateEnterEvent(TStateEnum state)
        {
            if(_stateEvents.Length <= 0)
                return;
            
            _stateEvents.FirstOrDefault(s => s.State.Equals(state))?.OnStateEnter?.Invoke(state);
        }
        
        private void InvokeStateExitEvent(TStateEnum state)
        {
            if(_stateEvents.Length <= 0)
                return;
            
            _stateEvents.FirstOrDefault(s => s.State.Equals(state))?.OnStateExit?.Invoke(state);
        }

        #endregion
        
        #region Debug

        protected void StateChangedDebug(TStateEnum stateName)
        {
            if(_debugStateChange)
                Debug.Log($"[StateBehaviour [<color=pink>{this.name}</color>] entered state <color=yellow>{stateName}</color>");
        }

        #endregion
    }
}