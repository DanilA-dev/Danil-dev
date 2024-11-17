using System;
using UnityEngine;

namespace Entities
{
    public class PoolableEntity : MonoBehaviour, IEntity
    {
        public event Action<PoolableEntity> OnEntityRelease;
        public event Action<PoolableEntity> OnEntityDestroy;

        public GameObject GameObject => gameObject;

        private void OnDestroy() => OnEntityDestroy?.Invoke(this);
        public void Release() => OnEntityRelease?.Invoke(this);
    }
}