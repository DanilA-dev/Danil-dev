using System;

namespace D_Dev.StateMachine
{
    public class StateTransition<TStateEnum>  where TStateEnum : Enum
    {
        public TStateEnum ToState { get; set; }
        public IStateCondition Condition { get; }

        public StateTransition(TStateEnum toState, IStateCondition condition)
        {
            ToState = toState;
            Condition = condition;
        }
    }
}
