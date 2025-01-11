using System.Collections.Generic;
using D_Dev.UtilScripts.State_Machine.Predicates;
using D_Dev.UtilScripts.State_Machine.Transitions;

namespace D_Dev.UtilScripts.State_Machine
{
    public partial class StateMachine
    {
        public class StateNode
        {
            #region Properties

            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            #endregion

            #region Construct

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            #endregion

            #region Public

            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));
            }

            #endregion
        }
    }
}