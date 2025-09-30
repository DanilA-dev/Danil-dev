using UnityEngine;

namespace D_Dev.InteractableSystem
{
    public interface IInteractable
    {
        public void StartInteract(GameObject interactor);
        public bool CanInteract(GameObject interactor);
    }
}
