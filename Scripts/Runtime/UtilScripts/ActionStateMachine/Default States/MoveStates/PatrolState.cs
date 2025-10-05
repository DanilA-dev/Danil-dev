using Sirenix.OdinInspector;
using UnityEngine;

namespace D_dev.UltimateStateController.Default_States.MoveStates
{
    #region Enums

    public enum PatrolMode
    {
        Loop,
        PingPong
    }

    #endregion

    [System.Serializable]
    public class PatrolState : BaseMoveState
    {
        #region Fields

        [SerializeField] private PatrolMode _patrolMode;
        [SerializeField] private TargetInfo.TargetInfo[] _patrolPoints;
        [SerializeField, ReadOnly] private int _currentIndex;
        [SerializeField] private bool _isReversing;

        #endregion

        #region Overrides

        protected override Vector3 GetTargetPosition()
            => _patrolPoints[_currentIndex].GetTargetPosition();

        protected override void OnTargetReached()
            => MoveToNextPoint();
        
        #endregion

        #region Private
        private void MoveToNextPoint()
        {
            switch (_patrolMode)
            {
                case PatrolMode.Loop:
                    _currentIndex = (_currentIndex + 1) % _patrolPoints.Length;
                    break;
                case PatrolMode.PingPong:
                    if (_isReversing)
                    {
                        _currentIndex--;
                        if (_currentIndex < 0)
                        {
                            _currentIndex = 1;
                            _isReversing = false;
                        }
                    }
                    else
                    {
                        _currentIndex++;
                        if (_currentIndex >= _patrolPoints.Length)
                        {
                            _currentIndex = _patrolPoints.Length - 2;
                            _isReversing = true;
                        }
                    }
                    break;
            }
        }

        #endregion
    }
}
