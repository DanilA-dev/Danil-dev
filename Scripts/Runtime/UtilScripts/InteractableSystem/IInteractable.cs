using UnityEngine;

namespace D.dev.InteractableSystem
{
    public interface IInteractable
    {
        public void StartInteract(GameObject interactor);
        public bool CanInteract(GameObject interactor);
    }
}
