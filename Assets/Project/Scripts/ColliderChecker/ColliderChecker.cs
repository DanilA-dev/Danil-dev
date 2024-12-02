using System.Linq;
using Sirenix.OdinInspector;
using Tag_System;
using UnityEngine;

namespace Project.Scripts.ColliderChecker
{
    [System.Serializable]
    public class ColliderChecker
    {
        [Title("Checks")]
        [SerializeField] private bool _checkLayer;
        [ShowIf(nameof(_checkLayer))]
        [SerializeField] private LayerMask _checkLayerMask;
        [SerializeField] private bool _checkTag;
        [ShowIf(nameof(_checkTag))]
        [SerializeField] private Tag[] _checkTags;
        [Space]
        [Title("Ignore")]
        [SerializeField] private bool _ignoreTrigger;
        [SerializeField] private bool _ignoreColliders;
        [ShowIf(nameof(_ignoreColliders))]
        [SerializeField] private Collider[] _ignoredColliders;
        [SerializeField] private bool _ignoreLayer;
        [ShowIf(nameof(_ignoreLayer))]
        [SerializeField] private LayerMask _ignoreLayerMask;
        [SerializeField] private bool _ignoreTag;
        [ShowIf(nameof(_ignoreTag))]
        [SerializeField] private Tag[] _ignoreTags;

        public bool IsColliderPassed(Collider collider)
        {
            bool checkPassed = true;
            bool ignorePassed = true;
            
            if (_checkLayer && ((1 << collider.gameObject.layer) & _checkLayerMask) == 0)
                checkPassed = false;

            if (_checkTag && !collider.gameObject.HasTags(_checkTags))
                checkPassed = false;

            if (_ignoreTrigger && collider.isTrigger)
                ignorePassed = false;

            if (_ignoreColliders && _ignoredColliders.Any(c => c.Equals(collider)))
                ignorePassed = false;
            
            if (_ignoreLayer && ((1 << collider.gameObject.layer) & _ignoreLayerMask) != 0)
                ignorePassed = false;

            if (_ignoreTag && collider.gameObject.HasTags(_ignoreTags))
                ignorePassed = false;
            
            return checkPassed && ignorePassed;
        }
        
    }
}