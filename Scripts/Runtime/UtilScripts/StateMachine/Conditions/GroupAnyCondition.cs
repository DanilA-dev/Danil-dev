using System.Linq;

namespace D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine
{
    public class GroupAnyCondition : IStateCondition
    {
        private IStateCondition[] _conditions;

        public GroupAnyCondition(IStateCondition[] conditions)
        {
            _conditions = conditions;
        }

        public bool IsMatched()
        {
            return _conditions.Any(c => c.IsMatched());
        }
    }
}