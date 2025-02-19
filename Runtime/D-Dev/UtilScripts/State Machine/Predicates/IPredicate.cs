namespace D_Dev.UtilScripts.State_Machine.Predicates
{
    public interface IPredicate
    {
        public bool CanBeUpdated { get; set; }
        public bool Evaluate();
    }
}