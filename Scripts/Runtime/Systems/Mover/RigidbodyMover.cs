using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.Mover
{
    [System.Serializable]
    public class RigidbodyMover : IMoverStrategy
    {
        #region Enums
        public enum RigidbodyMoverType
        {
            AddForce = 0,
            Velocity = 1,
            MovePosition = 2
        }

        #endregion

        #region Fields

        [SerializeField] private Rigidbody _owner;
        [SerializeField] private RigidbodyMoverType _moveType;
        [ShowIf(nameof(_moveType), RigidbodyMoverType.AddForce)]
        [SerializeField] private ForceMode _forceMode;

        #endregion

        #region Properties
        public RigidbodyMoverType MoveType { get => _moveType; set => _moveType = value; }

        #endregion

        #region Public

        public Vector3 GetCurrentPosition() => _owner.position;

        public void MoveTowards(Vector3 target, float speed, float deltaTime)
        {
            switch (_moveType)
            {
                case RigidbodyMoverType.AddForce:
                    _owner.AddForce(target * speed, _forceMode);
                    break;
                case RigidbodyMoverType.Velocity:
                    _owner.linearVelocity = target * speed * Time.deltaTime;
                    break;
                case RigidbodyMoverType.MovePosition:
                    _owner.MovePosition(Vector3.MoveTowards(_owner.position, target, speed * deltaTime));
                    break;
            }
        }
        public bool IsAtPosition(Vector3 target, float tolerance) 
            => Vector3.Distance(_owner.position, target) <= tolerance;

        #endregion
    }
}
