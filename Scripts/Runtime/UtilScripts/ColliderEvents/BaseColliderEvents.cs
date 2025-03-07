using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.UtilScripts.ColliderEvents
{
    public abstract class BaseColliderEvents : MonoBehaviour
    {
        #region Fields

        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected ColliderChecker.ColliderChecker _colliderChecker;
        [Space]
        [SerializeField] protected bool _checkEnter;
        [SerializeField] protected bool _checkExit;
        [Space]
        [ShowIf(nameof(_checkEnter))]
        [SerializeField] protected UnityEvent<Collider> _onEnter;
        [ShowIf(nameof(_checkExit))]
        [SerializeField] protected UnityEvent<Collider> _onExit;

        #endregion

        #region Properties

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

        #endregion

        #region Monobehaviour

        private void Awake() => InitColliderEvents();

        #endregion

        #region Abstract
        protected abstract void InitColliderEvents();

        #endregion
    }
}