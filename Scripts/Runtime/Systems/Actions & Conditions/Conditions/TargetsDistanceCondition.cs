using D_Dev.Base;
using D_Dev.PositionRotationConfig;
using UnityEngine;

namespace D_Dev.Conditions
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
        [SerializeReference] private BasePositionSettings _fromTarget = new();
        [SerializeReference] private BasePositionSettings _toTarget = new();
        [SerializeField] private float _distance;

        #endregion

        #region ICondition

        public bool IsConditionMet()
        {
            if(_checkType == DistanceCheckType.Less)
                return Vector3.Distance(_fromTarget.GetPosition(), _toTarget.GetPosition()) <= _distance;
            else
                return Vector3.Distance(_fromTarget.GetPosition(), _toTarget.GetPosition()) >= _distance;
        }

        public void Reset() {}

        #endregion
    }
}
