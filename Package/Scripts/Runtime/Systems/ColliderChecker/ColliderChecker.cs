using System.Linq;
using D_Dev.TagSystem;
using D_Dev.TagSystem.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.ColliderChecker
{

    #region Enums

    public enum CollisionDimension
    {
        Collider3d = 0,
        Collider2d = 1
    }

    #endregion

    [System.Serializable]
    public class ColliderChecker
    {
        #region Fields

        [Title("Dimension")]
        [SerializeField] private CollisionDimension _collisionDimension;
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
        [ShowIf(nameof(_collisionDimension), CollisionDimension.Collider3d)]
        [SerializeField] private Collider[] _ignoredColliders;
        [ShowIf(nameof(_ignoreColliders))]
        [ShowIf(nameof(_collisionDimension), CollisionDimension.Collider2d)]
        [SerializeField] private Collider2D[] _ignoredColliders2D;
        [SerializeField] private bool _ignoreLayer;
        [ShowIf(nameof(_ignoreLayer))]
        [SerializeField] private LayerMask _ignoreLayerMask;
        [SerializeField] private bool _ignoreTag;
        [ShowIf(nameof(_ignoreTag))]
        [SerializeField] private Tag[] _ignoreTags;
        [Title("Debug")]
        [SerializeField] protected bool _debugColliders;

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

        public Collider2D[] IgnoredColliders2D
        {
            get => _ignoredColliders2D;
            set => _ignoredColliders2D = value;
        }

        public CollisionDimension CollisionDimension
        {
            get => _collisionDimension;
            set => _collisionDimension = value;
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
            if (collider == null || collider.gameObject == null)
                return false;

            bool checkPassed = true;
            bool ignorePassed = true;

            if (_checkLayer && ((1 << collider.gameObject.layer) & _checkLayerMask) == 0
                || _checkTag && !collider.gameObject.HasTags(_checkTags))
                checkPassed = false;

            if (_ignoreTrigger && collider.isTrigger
                || _ignoreColliders && _ignoredColliders != null && _ignoredColliders.Any(c => c.Equals(collider))
                || _ignoreLayer && ((1 << collider.gameObject.layer) & _ignoreLayerMask) != 0
                || _ignoreTag && collider.gameObject.HasTags(_ignoreTags))
                ignorePassed = false;

            bool passed = checkPassed && ignorePassed;
            DebugCollider(collider, passed);
            return passed;
        }

        public bool IsColliderPassed(Collider2D collider2D)
        {
            if (collider2D == null || collider2D.gameObject == null)
                return false;

            bool checkPassed = true;
            bool ignorePassed = true;

            if (_checkLayer && ((1 << collider2D.gameObject.layer) & _checkLayerMask) == 0
                || _checkTag && !collider2D.gameObject.HasTags(_checkTags))
                checkPassed = false;

            if (_ignoreTrigger && collider2D.isTrigger
                || _ignoreColliders && _ignoredColliders2D != null && _ignoredColliders2D.Any(c => c.Equals(collider2D))
                || _ignoreLayer && ((1 << collider2D.gameObject.layer) & _ignoreLayerMask) != 0
                || _ignoreTag && collider2D.gameObject.HasTags(_ignoreTags))
                ignorePassed = false;

            bool passed = checkPassed && ignorePassed;
            DebugCollider(collider2D, passed);
            return passed;
        }

        #endregion
        
        #region Debug

        private void DebugCollider(Collider collider, bool isPassed)
        {
            string color = isPassed ? "green" : "red";
            string result = isPassed ? "is passed" : "don't passed";

            if(_debugColliders)
                Debug.Log($"{collider.name}, collider <color={color}> {result} </color>");
        }

        private void DebugCollider(Collider2D collider2D, bool isPassed)
        {
            string color = isPassed ? "green" : "red";
            string result = isPassed ? "is passed" : "don't passed";

            if(_debugColliders)
                Debug.Log($"{collider2D.name}, collider2D <color={color}> {result} </color>");
        }

        #endregion
    }
}
