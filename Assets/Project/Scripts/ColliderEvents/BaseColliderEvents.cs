using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Scripts.ColliderEvents
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
        [Space] 
        [SerializeField] protected bool _debugColliders;

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

        #region Debug

        protected void DebugCollider(Collider collider, bool isPassed)
        {
            string color = isPassed ? "green" : "red";
            string result = isPassed ? "is passed" : "don't passed";
            
            if(_debugColliders)
                Debug.Log($"[{gameObject.name}] found collider {collider.name}, collider <color={color}> {result} </color>");
        }

        #endregion
    }
}