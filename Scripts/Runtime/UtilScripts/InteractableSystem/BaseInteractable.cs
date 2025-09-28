using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D.dev.InteractableSystem
{
    public abstract class BaseInteractable : MonoBehaviour, IInteractable
    {
        #region Fields

        [Header("Interaction Settings")]
        [SerializeField] protected bool _isInteractable = true;
        [SerializeField] protected float _interactionDistance = 2f;
        [FoldoutGroup("Events")]
        public UnityEvent<GameObject> OnInteractStart;
        [FoldoutGroup("Events")]
        public UnityEvent OnInteractEnd;
        
        #endregion

        #region Properties

        public bool IsInteracting { get; protected set; }

        #endregion
        
        #region Virtual

        public virtual bool CanInteract(GameObject interactor)
        {
            if (!_isInteractable)
                return false;

            if (interactor == null)
                return false;

            float distance = Vector3.Distance(transform.position, interactor.transform.position);
            return distance <= _interactionDistance;
        }

        public void StartInteract(GameObject interactor)
        {
            if (!CanInteract(interactor))
                return;

            IsInteracting = true;
            OnInteract(interactor);
            OnInteractStart?.Invoke(interactor);
        }

        public void StopInteract(GameObject interactor)
        {
            if (!IsInteracting)
                return;

            IsInteracting = false;
            OnStopInteract(interactor);
            OnInteractEnd?.Invoke();
        }

        protected virtual void OnInteract(GameObject interactor) {}
        protected virtual void OnStopInteract(GameObject interactor) {}

        #endregion

        #region Gizmos

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _interactionDistance);
        }

        #endregion
    }
}
