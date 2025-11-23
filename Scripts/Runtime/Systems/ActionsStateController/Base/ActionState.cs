using System;
using System.Collections.Generic;
using System.Linq;
using D_Dev.Base;
using D_Dev.ScriptableVaiables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.ActionsStateController.Base
{
    [System.Serializable]
    public class ActionState
    {
        #region Enums

        public enum Phase
        {
            OnEnter,
            OnUpdate,
            OnFixedUpdate,
            OnExit
        }

        #endregion
        
        #region Classes

        [System.Serializable]
        public class StateActionPhase
        {
            [SerializeField] private ActionState.Phase _phase;
            [SerializeReference] private ActionGroup _group = new();

            public ActionState.Phase Phase => _phase;
            public ActionGroup Group => _group;
            public bool IsComplete => _group != null && _group.IsAllActionsCompleted;
        }

    
        #endregion

        #region Fields

        [SerializeField] private string _stateName;
        [SerializeField] private StringScriptableVariable _stateID;
        [SerializeField] private bool _hasExitTime;
        [ShowIf("_hasExitTime")]
        [SerializeField] private float _exitTime;

        [FoldoutGroup("Actions", HideWhenChildrenAreInvisible = true), PropertyOrder(1)] 
        [SerializeReference] private List<StateActionPhase> _actionGroups = new();
        [FoldoutGroup("Actions", HideWhenChildrenAreInvisible = true), PropertyOrder(0)] 
        [SerializeField] private bool _waitForActionsCompletion;
        [FoldoutGroup("Actions", HideWhenChildrenAreInvisible = true), PropertyOrder(0)] 
        [ShowIf("_waitForActionsCompletion")]
        [SerializeField] private StringScriptableVariable _nextStateAfterCompletion;
        [FoldoutGroup("Transitions", HideWhenChildrenAreInvisible = true)]
        [SerializeField] private StateTransition[] _transitions;
        [Space]
        [FoldoutGroup("Events", HideWhenChildrenAreInvisible = true), PropertyOrder(3)]
        public UnityEvent OnEnterEvent;
        [FoldoutGroup("Events", HideWhenChildrenAreInvisible = true), PropertyOrder(3)]
        public UnityEvent OnExitEvent;
        
        
        public event Action<StringScriptableVariable> OnActionsCompleted;

        #endregion

        #region Properties

        public bool HasExitTime => _hasExitTime;
        public float ExitTime => _exitTime;
        public StringScriptableVariable StateID => _stateID;
        public StateTransition[] Transitions => _transitions;

        public string StateName => _stateName;

        #endregion

        #region Public

        public void Enter()
        {
            ResetConditions();
            ResetActions();
            ExecuteActions(Phase.OnEnter);
            OnEnterEvent?.Invoke();
        }

        public void Update()
        {
            ExecuteActions(Phase.OnUpdate);
            CheckActionsCompletion();
        }

        public void FixedUpdate()
        {
            ExecuteActions(Phase.OnFixedUpdate);
        }

        public void Exit()
        {
            ExecuteActions(Phase.OnExit);
            OnExitEvent?.Invoke();
        }

        #endregion

        #region Private

        private void ResetConditions()
        {
            if (_transitions == null)
                return;

            foreach (var transition in _transitions)
            {
                if (transition.Conditions == null)
                    continue;

                foreach (var condition in transition.Conditions)
                {
                    condition?.Reset();
                }
            }
        }

        private void ResetActions()
        {
            if (_actionGroups == null)
                return;

            foreach (var entry in _actionGroups)
            {
                if (entry.Group?.Actions == null)
                    continue;

                foreach (var action in entry.Group.Actions)
                   action.Undo();
            }
        }

        private void ExecuteActions(Phase phase)
        {
            if (_actionGroups == null)
                return;

            foreach (var entry in _actionGroups.Where(e => e.Phase == phase))
                entry.Group.ExecuteActions();
        }

        private void CheckActionsCompletion()
        {
            if (!_waitForActionsCompletion || _actionGroups == null || OnActionsCompleted == null)
                return;

            if (_actionGroups.All(e => e.Group.IsAllActionsCompleted))
                OnActionsCompleted?.Invoke(_nextStateAfterCompletion);
        }

        #endregion
    }
}
