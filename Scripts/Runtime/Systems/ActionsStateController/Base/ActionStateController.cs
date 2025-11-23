using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using D_Dev.ScriptableVaiables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.ActionsStateController.Base
{
    public class ActionStateController : MonoBehaviour
    {
        #region Fields

        [PropertySpace(10)] 
        [SerializeReference, PropertyOrder(1)] private List<ActionState> _states = new();
        [SerializeField] private StringScriptableVariable _initialState;
        [SerializeField, ReadOnly] private StringScriptableVariable _currentStateID;
        [SerializeField] private float _transitionCheckInterval = 0.1f;
        [Title("Debug")]
        [SerializeField] private bool _debugStates;
        
        private Dictionary<StringScriptableVariable, ActionState> _statesDic;
        private CancellationTokenSource  _tokenSource;
        private float _nextTransitionCheckTime;

        #endregion

        #region Properties

        public ActionState CurrentState { get; private set; }

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
            
            if(_states == null || _states.Count == 0)
                return;
            
            foreach (var actionState in _states)
                actionState.OnActionsCompleted -= ChangeStateById;
        }

        #endregion

        #region Public

        public void ChangeStateById(StringScriptableVariable stateId)
        {
            if (_statesDic.TryGetValue(stateId, out var nextState))
                ChangeStateAsync(nextState);
        }

        #endregion

        #region Private

        private void InitStatesDictionary()
        {
            if(_states == null || _states.Count == 0)
                return;

            _statesDic = new Dictionary<StringScriptableVariable, ActionState>(_states.Count);
            foreach (var state in _states)
            {
                if (state?.StateID != null && !_statesDic.ContainsKey(state.StateID))
                {
                    _statesDic.Add(state.StateID, state);
                    state.OnActionsCompleted -= ChangeStateById;
                    state.OnActionsCompleted += ChangeStateById;
                }
            }
        }
        
        private void SetInitialState()
        {
            if (_states == null || _states.Count == 0)
                return;

            if (_statesDic.TryGetValue(_initialState, out var initialState))
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
        
        private async UniTask ChangeStateAsync(ActionState nextState)
        {
            if (CurrentState == nextState)
                return;

            _tokenSource.Cancel();
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

            if(_debugStates)
                Debug.Log($"[StateController : {gameObject.name}] {CurrentState.StateName} → {nextState.StateName}");

            CurrentState.Exit();
            CurrentState = nextState;
            _currentStateID = nextState.StateID;
            CurrentState.Enter();
        }

        #endregion
    }
}
