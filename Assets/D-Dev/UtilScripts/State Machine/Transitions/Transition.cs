using D_Dev.UtilScripts.State_Machine.Predicates;

namespace D_Dev.UtilScripts.State_Machine.Transitions
{
    public class Transition : ITransition
    {
        public IState To { get; }
        public IPredicate Condition { get; }
        
        public Transition(IState to, IPredicate condition)
        {
            To = to;
            Condition = condition;
        }
    }
}