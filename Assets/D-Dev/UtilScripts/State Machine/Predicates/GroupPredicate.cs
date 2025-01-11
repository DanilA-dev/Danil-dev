using System.Collections.Generic;
using System.Linq;

namespace D_Dev.UtilScripts.State_Machine.Predicates
{
    public class GroupPredicate : IPredicate
    {
        private List<IPredicate> _predicates;

        public bool CanBeUpdated { get; set; } = true;
        public GroupPredicate(List<IPredicate> predicates)
        {
            _predicates = predicates;
        }

        public bool Evaluate()
        {
            return CanBeUpdated && _predicates.All(p => p.Evaluate());
        }
    }
}