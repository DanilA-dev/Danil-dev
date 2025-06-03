using UnityEngine;

namespace D_dev.Scripts.EventHandler
{
    public class CustomEventInvoker : MonoBehaviour
    {
        #region Fields

        [SerializeField] private bool _invokeOnStart;
        [SerializeField] private CustomEventType _event;
        [Space] 
        [SerializeField] private bool _debug;
       

        #endregion
        
        #region Monobehaviour

        private void Start()
        {
            if(_invokeOnStart)
                Raise();
        }

        #endregion
        
        #region Public

        public void Raise()
        {
            CustomEventHandler.Invoke(_event, () =>
            {
                if(!_debug)
                    return;

                Debug.Log(
                    $"<color=yellow>[EventInvoker_{gameObject.name}]</color> raised event <color=pink>{_event.ToString()}</color>");

            });
        }

        #endregion
    }
}