using System;
using UnityEngine;

namespace SpawnSettings
{
    [Serializable]
    public class TransformSpawnSettings : ISpawnSettings
    {
        [field: SerializeField] public Transform Transform { get; private set; }

        public GameObject Spawn(GameObject gameObject)
        {
            return GameObject.Instantiate(gameObject, Transform.position, Transform.rotation);
        }
    }
}