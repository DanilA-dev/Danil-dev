using D_Dev.UtilScripts.State_Machine.Predicates;

namespace D_Dev.UtilScripts.State_Machine.Transitions
{
    public interface ITransition
    {
        public IState To { get; }
        public IPredicate Condition { get; }
    }
}