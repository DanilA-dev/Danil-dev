using System.Collections.Generic;
using UnityEngine;

namespace D_Dev.CustomEventManager
{
    public abstract class BaseParamEventInvoker<T> : MonoBehaviour
    {
        #region Fields

        [SerializeReference] private List<BaseEventNameType> _eventNameTypes = new();
        [SerializeField] private bool _invokeOnStart;
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
                Raise();
        }

        #endregion
        
        #region Public

        public void Raise()
        {
            if (_eventNameTypes != null && _eventNameTypes.Count > 0)
            {
                foreach (var eventNameType in _eventNameTypes)
                {
                    EventManager.Invoke(eventNameType.EventName, Param, () =>
                    {
                        if(!_debug)
                            return;
                
                        string paramInfo = Param == null ? "null" : Param.GetType().Name;
                        Debug.Log($"<color=yellow>[EventInvoker_{gameObject.name}]</color> raised event <color=orange>{eventNameType.EventName}</color>," +
                                  $" with parameter {paramInfo} : {Param?.ToString()}");
                    });
                }
            }
        }
        
        public void Raise(T t = default)
        {
            if (_eventNameTypes != null && _eventNameTypes.Count > 0)
            {
                foreach (var eventNameType in _eventNameTypes)
                {
                    EventManager.Invoke(eventNameType.EventName, t, () =>
                    {
                        if(!_debug)
                            return;
                
                        string paramInfo = t == null ? "null" : t.GetType().Name;
                        Debug.Log($"<color=yellow>[EventInvoker_{gameObject.name}]</color> raised event <color=orange>{eventNameType.EventName}</color>," +
                                  $" with parameter {paramInfo} : {t?.ToString()}");
                    });
                }
            }
        }

        #endregion
    }
}
