using System.Collections.Generic;
using System.Linq;

namespace  CustomFSM.Preicate
{
    public class SequencePredicate : IPredicate
    {
        private List<IPredicate> _predicates;

        private bool _isReset;
        
        public bool CanBeUpdated { get; set; } = true;
        
        public SequencePredicate(List<IPredicate> predicates)
        {
            _predicates = predicates;
        }


        public bool Evaluate()
        {
            if (!_isReset)
            {
                _predicates.ForEach(p => p.CanBeUpdated = false);
                _predicates.First().CanBeUpdated = true;
                _isReset = true;
            }
            
            for (int i = 0; i < _predicates.Count; i++)
            {
                int nextIndex = i + 1;
                if (nextIndex < _predicates.Count)
                {
                    if (_predicates[i].Evaluate())
                        _predicates[nextIndex].CanBeUpdated = true;
                }
            }

            if (_predicates.All(p => p.Evaluate()))
            {
                _isReset = false;
                return true;
            }

            return false;
        }
    }
}