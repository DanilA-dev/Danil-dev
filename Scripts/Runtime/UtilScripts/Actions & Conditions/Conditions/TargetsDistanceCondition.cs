using UnityEngine;

namespace D_dev.Conditions
{
    [System.Serializable]
    public class TargetsDistanceCondition : ICondition
    {
        #region Enums

        public enum DistanceCheckType
        {
            Less,
            Greater,
        }

        #endregion
        
        #region Fields

        [SerializeField] private DistanceCheckType _checkType;
        [SerializeField] private TargetInfo.TargetInfo _fromTarget;
        [SerializeField] private TargetInfo.TargetInfo _toTarget;
        [SerializeField] private float _distance;

        #endregion

        #region ICondition

        public bool IsConditionMet()
        {
            if(_checkType == DistanceCheckType.Less)
                return Vector3.Distance(_fromTarget.GetTargetPosition(), _toTarget.GetTargetPosition()) <= _distance;
            else
                return Vector3.Distance(_fromTarget.GetTargetPosition(), _toTarget.GetTargetPosition()) >= _distance;
        }

        public void Reset() {}

        #endregion
    }
}
