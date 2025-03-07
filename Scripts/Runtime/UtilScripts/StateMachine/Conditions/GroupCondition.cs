using System.Linq;

namespace D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine
{
    public class GroupCondition : IStateCondition
    {
        private IStateCondition[] _conditions;

        public GroupCondition(IStateCondition[] conditions)
        {
            _conditions = conditions;
        }

        public bool IsMatched()
        {
            return _conditions.All(c => c.IsMatched());
        }
    }
}