using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.EventHandler.Listeners
{
    public abstract class BaseParamCustomEventListener<T> : MonoBehaviour
    {
        #region Fields

        [SerializeField] private CustomEventType _event;
        [SerializeField] private UnityEvent<T> _onRaised;
        [Space] 
        [SerializeField] private bool _debug;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            CustomEventHandler.AddListener<T>(_event, OnRaised,
                onAddCallBack: (del) =>
            {
                if(!_debug)
                    return;

                Debug.Log(
                    $"<color=yellow>[EventListener_{gameObject.name}]</color> subscribed to event {_event.ToString()}");


            }, onFailCallback: (del) =>
                {
                    if(!_debug)
                        return;
                    
                    Debug.Log(
                        $"<color=yellow>[EventListener_{gameObject.name}]</color> fail to subscribe to event {_event.ToString()}," +
                        $" - Type mismatch: {typeof(T).Name} and {del.GetMethodInfo().Name}");
                    
                });
        }

        private void OnDestroy()
        {
            CustomEventHandler.RemoveListener<T>(_event, OnRaised,
                onRemoveCallback: (del) =>
                {
                    if(!_debug)
                        return;

                    Debug.Log(
                        $"<color=yellow>[EventListener_{gameObject.name}]</color> unsubscribed from event <color=pink>{_event.ToString()}</color>");


                }, onFailCallback: (del) =>
                {
                    if(!_debug)
                        return;
                    
                    Debug.Log(
                        $"<color=yellow>[EventListener_{gameObject.name}]</color> fail to unsubscribe from event <color=pink>{_event.ToString()}</color>," +
                        $" - Type mismatch: {typeof(T).Name} and {del.GetMethodInfo().Name}");
                    
                });
        }

        #endregion

        #region Listeners

        private void OnRaised(T t) => _onRaised?.Invoke(t);

        #endregion
    }
}
