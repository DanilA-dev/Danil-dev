using System;

namespace D_Dev.UtilScripts.State_Machine.Predicates
{
    public class FuncPredicate : IPredicate
    {
        private Func<bool> _func;

        public FuncPredicate(Func<bool> func)
        {
            _func = func;
        }

        public bool CanBeUpdated { get; set; } = true;
        public bool Evaluate() => _func.Invoke();
    }
}