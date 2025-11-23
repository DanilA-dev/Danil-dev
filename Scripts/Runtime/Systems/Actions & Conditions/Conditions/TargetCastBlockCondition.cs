using D_Dev.Raycaster;
using UnityEngine;

namespace D_dev.Conditions
{
    [System.Serializable]
    public class TargetCastBlockCondition : ICondition
    {
        #region Fields

        [SerializeField] private TargetInfo.TargetInfo _from;
        [SerializeField] private TargetInfo.TargetInfo _to;
        [Space]
        [SerializeField] private Linecaster _linecaster;
        [SerializeField] private bool _inverse;

        #endregion

        #region Public

        public bool IsConditionMet()
        {
            if (!_inverse)
                return _linecaster.IsIntersect(_from.GetTargetPosition(), _to.GetTargetPosition());
            else
                return!_linecaster.IsIntersect(_from.GetTargetPosition(), _to.GetTargetPosition());
        }

        public void Reset() {}

        #endregion
    }
}