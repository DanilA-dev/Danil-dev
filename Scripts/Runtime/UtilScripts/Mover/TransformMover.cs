    using UnityEngine;

namespace D_Dev.Mover
{
    public class TransformMover : BaseMover
    {
        #region Fields

        [SerializeField] private Transform _target;
        [SerializeField] private float _speed;
        #endregion

        #region Properties

        public Transform Target
        {
            get => _target;
            set => _target = value;
        }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }


        #endregion

        #region Protected

        protected override void OnMove(Vector3 direction)
        {
            transform.position += direction * _speed * Time.deltaTime;
        }

        #endregion
    }
}
