using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine
{
    public class StateMachine<TStateEnum> where TStateEnum : Enum
    {
        #region Fields

        private TStateEnum _currentState;
        
        private IState _current;
        private Dictionary<TStateEnum, IState> _states;
        private Dictionary<TStateEnum, StateTransition<TStateEnum>> _statesConditions;
        private CancellationTokenSource _tokenSource;
        
        private bool _isStateSwitching;
        
        public UnityAction<TStateEnum> OnStateEnter;
        public UnityAction<TStateEnum> OnStateExit;
            
        #endregion

        #region Properties
        public TStateEnum CurrentState => _currentState;

        #endregion

        #region Constructors

        public StateMachine()
        {
            _states = new();
            _statesConditions = new ();
            _tokenSource = new CancellationTokenSource();
        }

        #endregion

        #region Public

        public void AddState(TStateEnum stateEnum, IState state) => _states.TryAdd(stateEnum, state);
        public void AddTransition(TStateEnum fromState, TStateEnum toState, IStateCondition condition) 
            => _statesConditions.TryAdd(fromState, new StateTransition<TStateEnum>(toState, condition));

        public void OnUpdate()
        {
            _current?.OnUpdate();
            UpdateTransitions();
        }
        public void OnFixedUpdate() => _current?.OnFixedUpdate();
        
        public async UniTaskVoid ChangeState(TStateEnum newState)
        {
            if(_isStateSwitching)
                return;
            
            _isStateSwitching = true;
            if (!_states.TryGetValue(newState, out IState state))
            {
                Debug.Log($"[StateMachine] {GetType().Name}, State {newState} not found");
                return;
            }
            
            if (_current != null)
            {
                if(_current.ExitTime > 0)
                    await UniTask.Delay(TimeSpan.FromSeconds(_current.ExitTime), cancellationToken: _tokenSource.Token);
                
                _current.OnExit();
                OnStateExit?.Invoke(_currentState);
            }
            _current = state;
            _currentState = newState;
            _current.OnEnter();
            OnStateEnter?.Invoke(newState);
            _isStateSwitching = false;
        }

        #endregion

        #region Private

        private void UpdateTransitions()
        {
            if(_statesConditions.Count <= 0)
                return;

            foreach (var (stateEnum, transition) in _statesConditions)
            {
                if(!stateEnum.Equals(_currentState))
                    continue;
                
                if(transition.Condition.IsMatched() && !_isStateSwitching)
                    ChangeState(transition.ToState);
            }
        }

        #endregion
    }
}