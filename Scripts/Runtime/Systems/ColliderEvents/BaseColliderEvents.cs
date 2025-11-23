using System.Collections.Generic;
using D_Dev.ColliderChecker;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.ColliderEvents
{
    public abstract class BaseColliderEvents : MonoBehaviour
    {
        #region Fields

        [Title("Dimension")]
        [SerializeField] protected CollisionDimension _collisionDimension;
        [Title("Rigidbody")]
        [ShowIf(nameof(_collisionDimension), CollisionDimension.Collider3d)]
        [SerializeField] protected Rigidbody _rigidbody;
        [ShowIf(nameof(_collisionDimension), CollisionDimension.Collider2d)]
        [SerializeField] protected Rigidbody2D _rigidbody2D;
        [SerializeField] protected ColliderChecker.ColliderChecker _colliderChecker;
        [Space]
        [SerializeField] protected bool _checkEnter;
        [SerializeField] protected bool _checkExit;
        [Space]
        [ShowIf("@this._collisionDimension == CollisionDimension.Collider3d && this._checkEnter")]
        [SerializeField] protected UnityEvent<Collider> _onEnter;
        [ShowIf("@this._collisionDimension == CollisionDimension.Collider3d && this._checkExit")]
        [SerializeField] protected UnityEvent<Collider> _onExit;
        [ShowIf("@this._collisionDimension == CollisionDimension.Collider2d && this._checkEnter")]
        [SerializeField] protected UnityEvent<Collider2D> _onEnter2D;
        [ShowIf("@this._collisionDimension == CollisionDimension.Collider2d && this._checkExit")]
        [SerializeField] protected UnityEvent<Collider2D> _onExit2D;

        #endregion

        #region Properties

        public CollisionDimension Dimension
        {
            get => _collisionDimension;
            set => _collisionDimension = value;
        }

        public List<Collider> Colliders { get; private set; } = new();

        public List<Collider2D> Colliders2D { get; private set; } = new();

        public UnityEvent<Collider> OnEnter
        {
            get => _onEnter;
            set => _onEnter = value;
        }

        public UnityEvent<Collider> OnExit
        {
            get => _onExit;
            set => _onExit = value;
        }

        public UnityEvent<Collider2D> OnEnter2D
        {
            get => _onEnter2D;
            set => _onEnter2D = value;
        }

        public UnityEvent<Collider2D> OnExit2D
        {
            get => _onExit2D;
            set => _onExit2D = value;
        }

        #endregion

        #region Monobehaviour

        private void Awake() => InitColliderEvents();

        #endregion

        #region Abstract
        protected abstract void InitColliderEvents();

        #endregion
    }
}
