using System.Collections.Generic;
using CustomFSM.Preicate;
using CustomFSM.State;
using CustomFSM.Transition;

namespace CustomFSM.StateMachine
{
    public partial class StateMachine
    {
        public class StateNode
        {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }
            
            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new Transition.Transition(to, condition));
            }
        }
    }
}