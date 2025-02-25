using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.Scripts.Runtime.UtilScripts
{
    public class SimpleStateMachine<TStateEnum> where TStateEnum : Enum
    {
        #region Fields

        protected TStateEnum _startState; 
        protected TStateEnum _currentState;
        
        protected Dictionary<TStateEnum, IState> _states;
        protected IState _current;
        protected CancellationTokenSource _tokenSource;

        public UnityAction<TStateEnum> OnStateEnter;
        public UnityAction<TStateEnum> OnStateExit;
            
        #endregion

        #region Properties

        public TStateEnum CurrentState => _currentState;

        #endregion

        #region Constructors

        public SimpleStateMachine(TStateEnum startState)
        {
            _states = new();
            _tokenSource = new CancellationTokenSource();
        }

        #endregion

        #region Protected

        protected void AddState(TStateEnum stateEnum, IState state) => _states.TryAdd(stateEnum, state);
        protected void Update() => _current?.OnUpdate();
        protected void FixedUpdate() => _current?.OnFixedUpdate();

        #endregion

        #region Public

        public async UniTaskVoid ChangeState(TStateEnum newState)
        {
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
        }

        #endregion

        
        
       
    }
}