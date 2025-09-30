using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.EventHandler.Listeners
{
    public class CustomEventListener : MonoBehaviour
    {
        #region Fields

        [SerializeField] private CustomEventType _event;
        [SerializeField] private UnityEvent _onRaised;
        [Space]
        [SerializeField] private bool _debug;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            CustomEventHandler.AddListener(_event, OnRaised,
                onAddCallBack: (del) =>
            {
                if(!_debug)
                    return;

                Debug.Log(
                    $"<color=green>[EventListener_{gameObject.name}]</color> subscribed to event {_event.ToString()}");


            }, onFailCallback: (del) =>
                {
                    if(!_debug)
                        return;

                    Debug.Log(
                        $"<color=green>[EventListener_{gameObject.name}]</color> fail to subscribe to event {_event.ToString()}");


                });
        }

        private void OnDestroy()
        {
            CustomEventHandler.RemoveListener(_event, OnRaised,
                onRemoveCallback: (del) =>
                {
                    if(!_debug)
                        return;

                    Debug.Log(
                        $"<color=green>[EventListener_{gameObject.name}]</color> unsubscribed from event <color=pink>{_event.ToString()}</color>");


                }, onFailCallback: (del) =>
                {
                    if(!_debug)
                        return;

                    Debug.Log(
                        $"<color=green>[EventListener_{gameObject.name}]</color> fail to unsubscribe from event <color=pink>{_event.ToString()}</color>");
                });
        }

        #endregion

        #region Listeners

        private void OnRaised() => _onRaised?.Invoke();

        #endregion
    }
}
