using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_dev.ActionsStateController.Base
{
    [System.Serializable]
    public class StateTransition
    {
        #region Enums

        public enum ConditionsState
        {
            Any,
            All
        }

        #endregion
        
        #region Fields

        [SerializeField] private string _transitionName;
        [SerializeField] private ConditionsState _conditionState;
        [SerializeField] private int _priority;
        [SerializeReference] private D_dev.ICondition[] _conditions;
        [SerializeField] private StringScriptableVariable _transitionStateId;

        #endregion

        #region Properties

        public D_dev.ICondition[] Conditions => _conditions;

        public ConditionsState ConditionState => _conditionState;
        public StringScriptableVariable TransitionStateId => _transitionStateId;

        public int Priority => _priority;

        #endregion

        #region Public

        public bool CanTransition()
        {
            if (_conditions == null || _conditions.Length == 0)
                return false;

            if (_conditionState == ConditionsState.Any)
            {
                foreach (var condition in _conditions)
                {
                    if (condition != null && condition.IsConditionMet())
                        return true;
                }
                return false;
            }
            else
            {
                foreach (var condition in _conditions)
                {
                    if (condition == null || !condition.IsConditionMet())
                        return false;
                }
                return true;
            }
        }

        #endregion
    }
}
