using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.StateMachine
{
    public class StateMachine<TStateEnum> where TStateEnum : Enum
    {
        #region Fields

        private TStateEnum _currentState;
        
        private IState _current;
        private Dictionary<TStateEnum, IState> _states;
        private Dictionary<TStateEnum, List<StateTransition<TStateEnum>>> _statesConditions;
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
        {
            if(!_statesConditions.TryAdd(fromState, new List<StateTransition<TStateEnum>> {new(toState, condition) }))
                _statesConditions[fromState].Add(new(toState, condition));
        }

        public void RemoveTransition(TStateEnum keyState)
        {
            if(_statesConditions.ContainsKey(keyState))
                _statesConditions.Remove(keyState);
        }
        
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
                if(Equals(_currentState, newState))
                    return;
                
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
                
                foreach (var stateTransition in transition)
                {
                    if(stateTransition.Condition.IsMatched() && !_isStateSwitching)
                        ChangeState(stateTransition.ToState);
                }
            }
        }

        #endregion
    }
}
