using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.StateMachine
{
    public class StateMachine
    {
        #region Fields

        private string _currentState;
        
        private IState _current;
        private Dictionary<string, IState> _states;
        private Dictionary<string, List<StateTransition>> _statesConditions;
        private Dictionary<string, List<FixedStateTransition>> _fixedStatesConditions;
        private CancellationTokenSource _tokenSource;
        
        private bool _isStateSwitching;
        
        public UnityAction<string> OnStateEnter;
        public UnityAction<string> OnStateExit;
            
        #endregion

        #region Properties
        public string CurrentState => _currentState;

        #endregion

        #region Constructors

        public StateMachine()
        {
            _states = new();
            _statesConditions = new ();
            _fixedStatesConditions = new ();
            _tokenSource = new CancellationTokenSource();
        }

        #endregion

        #region Public

        public void AddState(string stateName, IState state) => _states.TryAdd(stateName, state);
        public void AddTransition(string fromState, string toState, IStateCondition condition)
        {
            if(!_statesConditions.TryAdd(fromState, new List<StateTransition> {new(toState, condition) }))
                _statesConditions[fromState].Add(new(toState, condition));
        }

        public void AddFixedTransition(string fromState, string toState, IFixedStateCondition condition)
        {
            if(!_fixedStatesConditions.TryAdd(fromState, new List<FixedStateTransition> {new(toState, condition) }))
                _fixedStatesConditions[fromState].Add(new(toState, condition));
        }

        public void RemoveTransition(string keyState)
        {
            if(_statesConditions.ContainsKey(keyState))
                _statesConditions.Remove(keyState);
        }

        public void RemoveFixedTransition(string keyState)
        {
            if(_fixedStatesConditions.ContainsKey(keyState))
                _fixedStatesConditions.Remove(keyState);
        }
        
        public void UpdateStates()
        {
            _current?.OnUpdate();
        }

        public void CheckTransitions()
        {
            UpdateTransitions();
        }

        public void UpdateStatesFixed()
        {
            _current?.OnFixedUpdate();
        }

   
        
        public async UniTaskVoid ChangeState(string newState)
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

            foreach (var (stateName, transition) in _statesConditions)
            {
                if(!stateName.Equals(_currentState))
                    continue;
                
                foreach (var stateTransition in transition)
                {
                    if(stateTransition.Condition.IsMatched() && !_isStateSwitching)
                        ChangeState(stateTransition.ToState);
                }
            }
        }
        
        public void UpdateFixedTransitions()
        {
            if(_fixedStatesConditions.Count <= 0)
                return;

            foreach (var (stateName, transition) in _fixedStatesConditions)
            {
                if(!stateName.Equals(_currentState))
                    continue;
                
                foreach (var fixedTransition in transition)
                {
                    if(fixedTransition.Condition.IsMatched() && !_isStateSwitching)
                        ChangeState(fixedTransition.ToState);
                }
            }
        }

        #endregion
    }
}
