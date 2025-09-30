using System.Collections;
using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.InteractableSystem.InteractableHandler
{
    public class InteractableDetector : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObjectScriptableVariable _lastSpottedInteractable;
        [SerializeField] private Raycaster.Raycaster _raycaster;
        [SerializeField] private float _updateRate = 0.1f;

        private IInteractable _currentInteractable;
        private WaitForSeconds _interval;
        private float _lastUpdateTime;

        #endregion

        #region Properties

        public IInteractable CurrentInteractable => _currentInteractable;

        #endregion

        #region MonoBehaviour

        private void Awake() => _interval = new WaitForSeconds(_updateRate);
        private void Start() => StartCoroutine(DetectInteractableRoutine(_updateRate));

        #endregion

        #region Coroutines

        private IEnumerator DetectInteractableRoutine(float updateRate)
        {
            while (true)
            {
                yield return _interval;
                if (_raycaster.IsHit(out Collider collider))
                {
                    var interactable = collider.GetComponent<IInteractable>();
                    if (interactable != null && interactable.CanInteract(gameObject))
                    {
                        _currentInteractable = interactable;
                        _lastSpottedInteractable.Value = collider.gameObject;
                    }
                    else
                    {
                        _currentInteractable = null;
                        _lastSpottedInteractable.Value = null;
                    }
                }
                else
                {
                    _currentInteractable = null;
                    _lastSpottedInteractable.Value = null;
                }
            }
        }

        #endregion
    }
}
