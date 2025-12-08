using UnityEngine;

namespace D_Dev.Mover
{
    public interface IMoverStrategy
    {
        public Vector3 GetCurrentPosition();
        public void MoveTowards(Vector3 target, float speed, float deltaTime);
        public bool IsAtPosition(Vector3 target, float tolerance);
    }
}
