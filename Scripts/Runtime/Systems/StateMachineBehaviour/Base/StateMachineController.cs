using System;
using System.Linq;
using D_Dev.PolymorphicValueSystem;
using D_Dev.StateMachine;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;


namespace D_Dev.StateMachineBehaviour
{
    public abstract class StateMachineController : MonoBehaviour
    {
        #region Fields

        [FoldoutGroup("Base Settings", order:100)]
        [SerializeField, ReadOnly] protected string _currentState;
        [FoldoutGroup("Base Settings", order:100)]
        [SerializeField] protected bool _debugStateChange;
        [Space]
        [FoldoutGroup("Base Settings", order:100)]
        [SerializeReference] protected PolymorphicValue<string> _startState;
        [Title("Events")]
        [FoldoutGroup("Base Settings", order:100)]
        [SerializeField] protected StateEvent[] _stateEvents;
        [FoldoutGroup("Base Settings", order:100)]
        public UnityEvent<string> OnAnyStateEnter;
        [FoldoutGroup("Base Settings", order:100)]
        public UnityEvent<string> OnAnyStateExit;

        [FoldoutGroup("Update Intervals", order: 200)]
        [SerializeField] private float _stateUpdateInterval = 0.05f;
        [FoldoutGroup("Update Intervals", order: 200)]
        [SerializeField] private float _transitionCheckInterval = 0.02f;

        protected StateMachine.StateMachine _stateMachine;
        private CompositeDisposable _disposables;

        #endregion

        #region Monobehaviour

        protected virtual void Awake()
        {
            _disposables = new CompositeDisposable();

            _stateMachine = new StateMachine.StateMachine();
            _stateMachine.OnStateEnter += state =>
            {
                _currentState = state;
                OnAnyStateEnter?.Invoke(state);
                InvokeStateEnterEvent(state);
                StateChangedDebug(state);
            };
            _stateMachine.OnStateExit += InvokeStateExitEvent;
            InitStates();

            Observable.Interval(TimeSpan.FromSeconds(_stateUpdateInterval))
                .Subscribe(_ => _stateMachine?.UpdateStates())
                .AddTo(_disposables);

            Observable.Interval(TimeSpan.FromSeconds(_transitionCheckInterval))
                .Subscribe(_ => _stateMachine?.CheckTransitions())
                .AddTo(_disposables);
        }

        protected virtual void Start() => ChangeState(_startState.Value);

        protected virtual void OnDestroy()
        {
            _disposables?.Dispose();
        }

        private void FixedUpdate()
        {
            _stateMachine?.UpdateStatesFixed();
            OnFixedUpdate();
        }

        #endregion

        #region Virtual/Abstract

        protected abstract void InitStates();
        protected virtual void OnFixedUpdate() {}

        #endregion

        #region Protected

        protected void AddState(string stateName, IState state) => _stateMachine?.AddState(stateName, state);
        protected void AddTransition(string[] fromStates, string toState, IStateCondition condition)
        {
            foreach (var fromState in fromStates)
                _stateMachine?.AddTransition(fromState, toState, condition);
        }
        
        protected void RemoveTransition(string keyState) => _stateMachine?.RemoveTransition(keyState);
        protected void ChangeState(string stateName) => _stateMachine.ChangeState(stateName);
        
        #endregion

        #region Private

        private void InvokeStateEnterEvent(string state)
        {
            if(_stateEvents.Length <= 0)
                return;
            
            _stateEvents.FirstOrDefault(s => s.State.Value.Equals(state))?.OnStateEnter?.Invoke(state);
        }
        
        private void InvokeStateExitEvent(string state)
        {
            if(_stateEvents.Length <= 0)
                return;
            
            _stateEvents.FirstOrDefault(s => s.State.Equals(state))?.OnStateExit?.Invoke(state);
        }

        #endregion
        
        #region Debug

        protected void StateChangedDebug(string stateName)
        {
            if(_debugStateChange)
                Debug.Log($"[StateBehaviour [<color=pink>{this.name}</color>] entered state <color=yellow>{stateName}</color>");
        }

        #endregion
    }
}
