using System;
using System.Linq;
using D_Dev.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace  D_Dev.StateMachineBehaviour
{
    public abstract class ModularStateMachineBehaviour<TStateEnum> : StateMachineBehaviour<TStateEnum> where TStateEnum : Enum
    {
        #region Fields

        [FoldoutGroup("Module Settings", 99)]
        [SerializeField] protected bool _findOnObject;
        [FoldoutGroup("Module Settings", 99)]
        [HideIf(nameof(_findOnObject))]
        [SerializeField] protected BaseModularState<TStateEnum>[] _states;

        #endregion

        #region Virtual/Abstract

        protected override void InitStates()
        {
            if (_findOnObject)
                _states = GetComponents<BaseModularState<TStateEnum>>();
            
            if (_states == null || _states.Length == 0)
                return;
            
            foreach (var state in _states)
                AddState(state.StateName, state);
            
            InitTransitions();
        }

        protected virtual void InitTransitions()
        {
            if (_states == null || _states.Length == 0)
                return;
            
            foreach (var state in _states)
            {
                if (state.Transitions == null || state.Transitions.Length == 0)
                    continue;
                
                foreach (var transition in state.Transitions)
                {
                    AddTransition(transition.FromStates, state.StateName, new FuncCondition(() =>
                        transition.Conditions.All(c => c.IsConditionMet())));
                }
            }
        }
        
        #endregion
    }
}