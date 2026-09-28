using UnityEngine;

namespace D_Dev.Base
{
    public interface IInteractable
    {
        public GameObject GameObject { get; }
        public bool CanBeStopped { get; set; }
        public bool StopOnFocusLost { get; set; }
        public bool IsDistanceBased { get; set; }
        public void StartInteract(GameObject interactor);
        public void StopInteract(GameObject interactor);
        public bool CanInteract(GameObject interactor);
        public void Focus(GameObject interactor);
        public void Unfocus(GameObject interactor);
    }
}
