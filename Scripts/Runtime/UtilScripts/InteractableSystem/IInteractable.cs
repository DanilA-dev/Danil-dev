using UnityEngine;

namespace D.dev.InteractableSystem
{
    public interface IInteractable
    {
        public void Interact(GameObject interactor);
        public bool CanInteract(GameObject interactor);
    }
}
