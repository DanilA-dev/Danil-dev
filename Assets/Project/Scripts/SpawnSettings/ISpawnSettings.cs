using System.Collections.Generic;
using UnityEngine;

namespace SpawnSettings
{
    public interface ISpawnSettings
    {
        public GameObject Spawn(GameObject gameObject);
    }
}