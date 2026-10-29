using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace D_Dev.InputSystem
{
    public class PointerClickPassThrough : MonoBehaviour, IPointerClickHandler
    {
        #region Fields

        private readonly List<RaycastResult> _results = new();

        #endregion

        #region IPointerClickHandler

        public void OnPointerClick(PointerEventData eventData)
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null || eventData.pointerPress != gameObject || !IsTap(eventData, eventSystem))
                return;

            _results.Clear();
            eventSystem.RaycastAll(eventData, _results);

            foreach (var result in _results)
            {
                if (result.gameObject == null || result.gameObject.transform.IsChildOf(transform))
                    continue;

                var handler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(result.gameObject);
                if (handler == null || handler.transform.IsChildOf(transform) || handler.TryGetComponent(out PointerClickPassThrough _))
                    continue;

                eventData.pointerCurrentRaycast = result;
                eventData.pointerPressRaycast = result;
                ExecuteEvents.Execute(handler, eventData, ExecuteEvents.pointerClickHandler);
                return;
            }
        }

        #endregion

        #region Private

        private static bool IsTap(PointerEventData eventData, EventSystem eventSystem)
        {
            var threshold = eventSystem.pixelDragThreshold;
            return (eventData.pressPosition - eventData.position).sqrMagnitude <= threshold * threshold;
        }

        #endregion
    }
}
