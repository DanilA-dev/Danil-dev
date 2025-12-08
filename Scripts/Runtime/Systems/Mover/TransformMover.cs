using UnityEngine;

namespace D_Dev.Mover
{
    [System.Serializable]
    public class TransformMover : IMoverStrategy
    {
        #region Fields

        [SerializeField] private Transform _owner;
        
        #endregion

        #region Public

        public Vector3 GetCurrentPosition() => _owner.position;
        
        public void MoveTowards(Vector3 target, float speed, float deltaTime) =>
            _owner.position = Vector3.MoveTowards(_owner.position, target, speed * deltaTime);
        public bool IsAtPosition(Vector3 target, float tolerance) =>
            Vector3.Distance(_owner.position, target) <= tolerance;

        #endregion
    }
}
