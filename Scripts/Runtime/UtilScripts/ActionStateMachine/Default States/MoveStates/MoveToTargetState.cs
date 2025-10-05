using D_dev.UltimateStateController.Default_States.MoveStates;
using UnityEngine;

namespace D_dev.UltimateStateController.Default_States
{
    [System.Serializable]
    public class MoveToTargetState : BaseMoveState
    {
        #region Fields

        [SerializeField] private TargetInfo.TargetInfo _target;

        #endregion

        #region Overrides

        protected override Vector3 GetTargetPosition()
        {
            return _target.GetTargetPosition();
        }

        #endregion
    }
}
