using UnityEngine;

namespace Tag_System
{
    public static class TagExtensions
    {
        public static bool HasTag(this GameObject gameObject, Tag tag)
        {
            return gameObject.TryGetComponent(out TagComponent tagComponent) 
                   && tagComponent.HasAnyTag(tag);
        }
    }
}