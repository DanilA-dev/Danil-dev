namespace CustomFSM.Preicate
{
    public interface IPredicate
    {
        public bool CanBeUpdated { get; set; }
        public bool Evaluate();
    }
}