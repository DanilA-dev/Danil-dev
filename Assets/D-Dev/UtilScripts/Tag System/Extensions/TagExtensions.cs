using UnityEngine;

namespace D_Dev.UtilScripts.Tag_System.Extensions
{
    public static class TagExtensions
    {
        public static bool HasTag(this GameObject gameObject, Tag tag)
        {
            return gameObject.TryGetComponent(out TagComponent tagComponent) 
                   && tagComponent.HasAnyTag(tag);
        }
        
        public static bool HasTags(this GameObject gameObject, Tag[] tags)
        {
            return gameObject.TryGetComponent(out TagComponent tagComponent)
                   && tagComponent.HasAnyTags(tags);
        }
    }
}