using D_Dev.Base;
using UnityEngine;

namespace D_Dev.CustomEventManager.Actions
{
    [System.Serializable]
    public class WaitEventAction : BaseAction
    {
        #region Fields

        [SerializeReference] private BaseEventNameType _eventNameType = new StringEventNameType();

        private string _subscribedEventName;

        #endregion

        #region Overrides

        public override void Execute()
        {
            if (_subscribedEventName != null || _eventNameType == null)
                return;

            var eventName = _eventNameType.EventName;
            if (string.IsNullOrEmpty(eventName))
                return;

            _subscribedEventName = eventName;
            EventManager.AddListener(_subscribedEventName, OnRaised);
        }

        public override void Undo()
        {
            Unsubscribe();
            base.Undo();
        }

        #endregion

        #region Private

        private void Unsubscribe()
        {
            if (_subscribedEventName == null)
                return;

            EventManager.RemoveListener(_subscribedEventName, OnRaised);
            _subscribedEventName = null;
        }

        #endregion

        #region Listeners

        private void OnRaised()
        {
            Unsubscribe();
            IsFinished = true;
        }

        #endregion
    }
}
