using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Entities.EntitiesInfo
{
    public class EntityInfo : SerializedScriptableObject
    {
        [OdinSerialize] public IEntity EntityPrefab { get; private set; }
    }
}