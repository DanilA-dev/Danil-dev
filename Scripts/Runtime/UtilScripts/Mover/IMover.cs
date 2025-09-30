using UnityEngine;

namespace D_Dev.Mover
{
    public interface IMover
    {
        public bool CanMove { get; set; }

        public void Move(Vector3 direction);
    }
}
