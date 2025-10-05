using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using D_Dev.ScriptableVaiables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_dev.UltimateStateController
{
    public class StateController : MonoBehaviour
    {
        #region Fields

        [SerializeReference] private BaseState[] _states;
        [SerializeField] private StringScriptableVariable _initialStateID;
        [SerializeField, ReadOnly] private StringScriptableVariable _currentStateID;
        [SerializeField] private float _transitionCheckInterval = 0.1f;
        
        private Dictionary<StringScriptableVariable, BaseState> _statesDic;
        private CancellationTokenSource  _tokenSource;
        private float _nextTransitionCheckTime;

        #endregion

        #region Properties

        public BaseState CurrentState { get; private set; }

        public StringScriptableVariable CurrentStateID => _currentStateID;

        #endregion

        #region Monobehavior

        private void Awake()
        {
            _tokenSource = new CancellationTokenSource();
        }

        private void Start()
        {
            InitStatesDictionary();
            SetInitialState();
        }

        private void Update()
        {
            CurrentState?.Update();
            
            if (Time.time >= _nextTransitionCheckTime)
            {
                _nextTransitionCheckTime = Time.time + _transitionCheckInterval;
                CheckTransitions();
            }
        }

        private void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }

        private void OnDestroy()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
        }

        #endregion

        #region Private

        private void InitStatesDictionary()
        {
            if(_states == null || _states.Length == 0)
                return;
            
            _statesDic = new Dictionary<StringScriptableVariable, BaseState>(_states.Length);
            foreach (var state in _states)
            {
                if (state?.StateID != null && !_statesDic.ContainsKey(state.StateID))
                    _statesDic.Add(state.StateID, state);
            }
        }
        
        private void SetInitialState()
        {
            if (_states == null || _states.Length == 0)
                return;

            if (_statesDic.TryGetValue(_initialStateID, out var initialState))
            {
                CurrentState = initialState;
                _currentStateID = initialState.StateID;
                CurrentState.Enter();
            }
        }
        
        private async void CheckTransitions()
        {
            if(CurrentState == null)
                return;

            if (CurrentState.Transitions == null || CurrentState.Transitions.Length == 0)
                return;

            int bestIndex = -1;
            int maxPriority = int.MinValue;
            for (int i = 0; i < CurrentState.Transitions.Length; i++)
            {
                var t = CurrentState.Transitions[i];
                if (t.CanTransition() && t.Priority > maxPriority)
                {
                    maxPriority = t.Priority;
                    bestIndex = i;
                }
            }

            if (bestIndex == -1)
                return;

            var bestTransition = CurrentState.Transitions[bestIndex];
            if (!_statesDic.TryGetValue(bestTransition.TransitionStateId, out var nextState))
                return;

            await ChangeStateAsync(nextState);
        }
        
        private async UniTask ChangeStateAsync(BaseState nextState)
        {
            if (CurrentState == nextState)
                return;

            _tokenSource.Cancel();
            _tokenSource.Dispose();
            _tokenSource = new CancellationTokenSource();

            if (CurrentState.HasExitTime)
            {
                try
                {
                    await UniTask.Delay(
                        System.TimeSpan.FromSeconds(CurrentState.ExitTime),
                        cancellationToken: _tokenSource.Token
                    );
                }
                catch (OperationCanceledException)
                {
                    return; 
                }
            }

            Debug.Log($"[StateController : {gameObject.name}] {CurrentState.StateID.name} → {nextState.StateID.name}");

            CurrentState.Exit();
            CurrentState = nextState;
            _currentStateID = nextState.StateID;
            CurrentState.Enter();
        }

        #endregion
    }
}
