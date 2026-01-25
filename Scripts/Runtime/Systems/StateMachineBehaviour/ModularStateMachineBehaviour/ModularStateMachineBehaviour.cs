using System;
using System.Linq;
using D_Dev.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace  D_Dev.StateMachineBehaviour
{
    public abstract class ModularStateMachineBehaviour<TStateEnum, TModularState> 
        : StateMachineBehaviour<TStateEnum> where TStateEnum : Enum where TModularState : BaseModularState<TStateEnum>
    {
        #region Fields

        [FoldoutGroup("Module Settings", 99)]
        [SerializeField] protected bool _findOnObject;
        [FoldoutGroup("Module Settings", 99)]
        [HideIf(nameof(_findOnObject))]
        [SerializeField] protected TModularState[] _states;

        #endregion

        #region Virtual/Abstract

        protected override void InitStates()
        {
            if (_findOnObject)
                _states = GetComponents<TModularState>();
            
            if (_states == null || _states.Length == 0)
                return;
            
            foreach (var state in _states)
                AddState(state.StateName, state);
            
            InitTransitions();
            OnStatesInitialized();
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
        
        protected virtual void OnStatesInitialized(){}
        
        #endregion
    }
}