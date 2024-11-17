using UnityEngine;

namespace SpawnSettings
{
    [System.Serializable]
    public class VectorSpawnSettings : ISpawnSettings
    {
        [SerializeField] private Vector3 _pos;
        
        public GameObject Spawn(GameObject gameObject)
        {
            return GameObject.Instantiate(gameObject, _pos, Quaternion.identity);
        }
    }
}