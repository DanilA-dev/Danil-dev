using System.Linq;
using Sirenix.OdinInspector;
using Tag_System;
using UnityEngine;

namespace Project.Scripts.ColliderChecker
{
    [System.Serializable]
    public class ColliderChecker
    {
        #region Fields

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

        #endregion

        #region Properties

        public bool CheckLayer
        {
            get => _checkLayer;
            set => _checkLayer = value;
        }

        public LayerMask CheckLayerMask
        {
            get => _checkLayerMask;
            set => _checkLayerMask = value;
        }

        public bool CheckTag
        {
            get => _checkTag;
            set => _checkTag = value;
        }

        public Tag[] CheckTags
        {
            get => _checkTags;
            set => _checkTags = value;
        }

        public bool IgnoreTrigger
        {
            get => _ignoreTrigger;
            set => _ignoreTrigger = value;
        }

        public bool IgnoreColliders
        {
            get => _ignoreColliders;
            set => _ignoreColliders = value;
        }

        public Collider[] IgnoredColliders
        {
            get => _ignoredColliders;
            set => _ignoredColliders = value;
        }

        public bool IgnoreLayer
        {
            get => _ignoreLayer;
            set => _ignoreLayer = value;
        }

        public LayerMask IgnoreLayerMask
        {
            get => _ignoreLayerMask;
            set => _ignoreLayerMask = value;
        }

        public bool IgnoreTag
        {
            get => _ignoreTag;
            set => _ignoreTag = value;
        }

        public Tag[] IgnoreTags
        {
            get => _ignoreTags;
            set => _ignoreTags = value;
        }

        #endregion

        #region Public

        public bool IsColliderPassed(Collider collider)
        {
            bool checkPassed = true;
            bool ignorePassed = true;
            
            if (_checkLayer && ((1 << collider.gameObject.layer) & _checkLayerMask) == 0
                || _checkTag && !collider.gameObject.HasTags(_checkTags))
                checkPassed = false;

            if (_ignoreTrigger && collider.isTrigger
                || _ignoreColliders && _ignoredColliders.Any(c => c.Equals(collider))
                || _ignoreLayer && ((1 << collider.gameObject.layer) & _ignoreLayerMask) != 0
                || _ignoreTag && collider.gameObject.HasTags(_ignoreTags))
                ignorePassed = false;
            
            return checkPassed && ignorePassed;
        }

        #endregion
    }
}