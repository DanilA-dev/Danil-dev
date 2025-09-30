using UnityEngine;
using UnityEngine.AI;

namespace D_Dev.Mover
{
    public class NavmeshMover : BaseMover
    {
        #region Fields

        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _speed;

        #endregion

        #region Properties

        public NavMeshAgent Agent
        {
            get => _agent;
            set => _agent = value;
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
            if (_agent != null)
            {
                _agent.speed = _speed;
                _agent.SetDestination(_agent.transform.position + direction);
            }
        }

        #endregion
    }
}
