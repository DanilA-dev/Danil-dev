using D_Dev.Mover;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_dev.Actions
{
    #region Enums

    public enum PatrolMode
    {
        Loop,
        PingPong
    }


    #endregion

    [System.Serializable]
    public class PatrolMovementAction : BaseAction
    {
        #region Fields

        [SerializeReference] private IMoverStrategy _movement;
        [Space]
        [SerializeField] private PatrolMode _patrolMode;
        [SerializeField] private TargetInfo.TargetInfo[] _patrolPoints;
        [SerializeField] private float _speed;
        [SerializeField] private float _reachDistance;
        [SerializeField, ReadOnly] private int _currentIndex;
        [SerializeField] private bool _isReversing;
        
        #endregion

        #region IAction

        public override void Execute()
        {
            if (_movement == null || _patrolPoints.Length == 0)
                return;

            var target = _patrolPoints[_currentIndex].GetTargetPosition();
            _movement.MoveTowards(target, _speed, Time.deltaTime);

            if (_movement.IsAtPosition(target, _reachDistance))
                MoveToNextPoint();
            
            if(_currentIndex == _patrolPoints.Length - 1)
                IsFinished = true;
        }

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
