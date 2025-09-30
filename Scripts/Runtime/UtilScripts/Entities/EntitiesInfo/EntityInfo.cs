using UnityEngine;

namespace D_Dev.Entities.EntitiesInfo
{
    [CreateAssetMenu(menuName = "D-Dev/Info/EntityInfo")]
    public class EntityInfo : ScriptableObject
    {
        [field: SerializeField] public GameObject EntityPrefab { get; private set; }
    }
}
