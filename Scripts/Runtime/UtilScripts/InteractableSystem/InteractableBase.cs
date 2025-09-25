using UnityEngine;
using UnityEngine.Events;

namespace D.dev.InteractableSystem
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        #region Fields

        [Header("Interaction Settings")]
        [SerializeField] protected bool _isInteractable = true;
        [SerializeField] protected float _interactionDistance = 2f;
        [Header("Events")]
        public UnityEvent<GameObject> OnInteractStart;
        public UnityEvent<GameObject> OnInteractEnd;

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

        public virtual void Interact(GameObject interactor)
        {
            if (!CanInteract(interactor))
                return;

            OnInteractStart?.Invoke(interactor);
            PerformInteraction(interactor);
        }

        protected abstract void PerformInteraction(GameObject interactor);

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
