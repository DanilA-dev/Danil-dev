using UnityEngine;

namespace D_Dev.UtilScripts.Mover
{
    public abstract class BaseMover : MonoBehaviour, IMover
    {
        [SerializeField] private bool _canMove = true;

        public bool CanMove { get => _canMove; set => _canMove = value; }

        public void Move(Vector3 direction)
        {
            if (CanMove)
                OnMove(direction);
        }

        protected abstract void OnMove(Vector3 direction);
    }
}