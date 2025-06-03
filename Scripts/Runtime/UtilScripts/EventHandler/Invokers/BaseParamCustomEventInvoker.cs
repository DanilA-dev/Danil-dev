using UnityEngine;

namespace D_dev.Scripts.EventHandler
{
    public abstract class BaseParamCustomEventInvoker<T> : MonoBehaviour
    {
        #region Fields

        [SerializeField] private bool _invokeOnStart;
        [SerializeField] private CustomEventType _event;
        [Space]
        [SerializeField] private bool _debug;

        #endregion

        #region Properties

        [field: SerializeField] public T Param { get; set; }

        #endregion
        
        #region Monobehaviour

        private void Start()
        {
            if(_invokeOnStart && Param != null)
                Raise(Param);
        }

        #endregion
        
        #region Public

        public void Raise(T t = default)
        {
            CustomEventHandler.Invoke(_event, t, () =>
            {
                if(!_debug)
                    return;
                
                string paramInfo = t == null ? "null" : t.GetType().ToString();
                Debug.Log($"<color=yellow>[EventInvoker_{gameObject.name}]</color> raised event <color=pink>{_event.ToString()}</color>," +
                          $" with parameter {paramInfo}");
            });
        }

        #endregion
    }
}