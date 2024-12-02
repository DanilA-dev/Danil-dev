using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tag_System
{
    public class TagComponent : MonoBehaviour
    {
        [SerializeField] private List<Tag> _tags;

        public bool HasAnyTag(Tag checkTag) => _tags.Any(t => t.Equals(checkTag));
        public bool HasAnyTags(Tag[] checkTags) => _tags.Any(tag => checkTags.Any(checkTag => checkTag == tag));

        public void AddTag(Tag tag)
        {
            if(!_tags.Contains(tag))
                _tags.Add(tag);
        }

        public void RemoveTag(Tag tag)
        {
            if(_tags.Contains(tag))
                _tags.Remove(tag);
        }
    }
}