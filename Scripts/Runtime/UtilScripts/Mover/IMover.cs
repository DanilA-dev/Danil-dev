using UnityEngine;

namespace D_Dev.UtilScripts.Mover
{
    public interface IMover
    {
        public bool CanMove { get; set; }

        public void Move(Vector3 direction);
    }
}