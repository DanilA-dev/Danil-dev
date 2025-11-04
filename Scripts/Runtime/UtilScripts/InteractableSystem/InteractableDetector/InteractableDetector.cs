using System.Collections;
using D_Dev.ScriptableVaiables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.InteractableSystem.InteractableDetector
{
    public class InteractableDetector : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObjectScriptableVariable _lastSpottedInteractable;
        [SerializeField] private Raycaster.Raycaster _raycaster;
        [SerializeField] private float _updateRate = 0.1f;

        [FoldoutGroup("Events")]
        public UnityEvent<IInteractable> OnInteractableFound;
        [FoldoutGroup("Events")]
        public UnityEvent OnInteractableLost;

        private IInteractable _currentInteractable;
        private WaitForSeconds _interval;

        #endregion

        #region Properties

        public IInteractable CurrentInteractable => _currentInteractable;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _interval = new WaitForSeconds(_updateRate);
        }

        private void Start()
        {
            StartCoroutine(DetectInteractableRoutine());
        }

        #endregion

        #region Coroutines

        private IEnumerator DetectInteractableRoutine()
        {
            while (true)
            {
                DetectInteractable();
                yield return _interval;
            }
        }

        #endregion

        #region Private

        private void DetectInteractable()
        {
            if (_raycaster.IsHit(out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out _currentInteractable) && _currentInteractable.CanInteract(gameObject))
                    _lastSpottedInteractable.Value = hit.collider.gameObject;
            }
            else
            {
                _currentInteractable = null;
                _lastSpottedInteractable.Value = null;
            }
        }

        #endregion

        #region Gizmos

        private void OnDrawGizmos() => _raycaster.OnGizmos();

        #endregion
    }
}
