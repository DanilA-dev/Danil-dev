using System;
using UnityEngine;

namespace D_Dev.TagSystem.Extensions
{
    public static class TagExtensions
    {
        public static Tag[] Tags(this GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out TagComponent tagComponent))
            {
                return tagComponent.Tags.ToArray();
            }

            Debug.LogError($"Dont have any tags on {gameObject.name}");
            return Array.Empty<Tag>();
        }
        
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