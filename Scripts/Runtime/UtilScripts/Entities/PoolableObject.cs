using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.EntitySpawner
{
    public class PoolableObject : MonoBehaviour
    {
        #region Fields

        [PropertyOrder(100)]
        [FoldoutGroup("Events")]
        public UnityEvent<PoolableObject> OnEntityRelease;
        [PropertyOrder(100)]
        [FoldoutGroup("Events")]
        public UnityEvent<PoolableObject> OnEntityDestroy;

        #endregion

        #region Monobehaviour

        private void OnDestroy() => OnEntityDestroy?.Invoke(this);
        public void Release() => OnEntityRelease?.Invoke(this);

        #endregion
    }
}
