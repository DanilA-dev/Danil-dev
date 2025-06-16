using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.UtilScripts.Mover
{
    public class RigidbodyMover : BaseMover
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

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private RigidbodyMoverType _moveType;
        [SerializeField] private float _forceAmount = 10f;

        [ShowIf(nameof(_moveType), RigidbodyMoverType.AddForce)]
        [SerializeField] private ForceMode _forceMode;

        #endregion

        #region Properties

        public Rigidbody Rigidbody => _rigidbody;
        public RigidbodyMoverType MoveType { get => _moveType; set => _moveType = value; }
        public float ForceAmount { get => _forceAmount; set => _forceAmount = value; }

        #endregion

        #region Protected


        protected override void OnMove(Vector3 direction)
        {
            switch (_moveType)
            {
                case RigidbodyMoverType.AddForce:
                    _rigidbody.AddForce(direction * _forceAmount, _forceMode);
                    break;
                case RigidbodyMoverType.Velocity:
                    _rigidbody.velocity = direction * _forceAmount * Time.deltaTime;
                    break;
                case RigidbodyMoverType.MovePosition:
                    _rigidbody.MovePosition(_rigidbody.position + direction * _forceAmount * Time.deltaTime);
                    break;
            }
        }

        #endregion
    }
}
