using CustomFSM.Preicate;
using CustomFSM.State;

namespace CustomFSM.Transition
{
    public interface ITransition
    {
        public IState To { get; }
        public IPredicate Condition { get; }
    }
}