using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Entities
{
    public class PoolableObject : MonoBehaviour
    {
        [PropertyOrder(100)]
        [FoldoutGroup("Events")]
        public UnityEvent<PoolableObject> OnEntityRelease;
        [PropertyOrder(100)]
        [FoldoutGroup("Events")]
        public UnityEvent<PoolableObject> OnEntityDestroy;

        private void OnDestroy() => OnEntityDestroy?.Invoke(this);
        public void Release() => OnEntityRelease?.Invoke(this);
    }
}