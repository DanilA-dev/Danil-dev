using D_Dev.Base;
using D_Dev.PolymorphicValueSystem;
using UnityEngine;

namespace D_Dev.ColliderEvents.Extensions.Conditions
{
    [System.Serializable]
    public class ColliderContainsTriggerObservable : ICondition
    {
        #region Fields

        [SerializeField] private Collider _targetCollider;
        [SerializeReference] private PolymorphicValue<TriggerColliderObservable> _triggerObservable;

        #endregion

        #region ICondition

        public bool IsConditionMet()
        {
            if (_triggerObservable == null || _triggerObservable.Value == null)
                return false;

            return _triggerObservable.Value.Colliders.Count != 0 
                   && _triggerObservable.Value.Colliders.Contains(_targetCollider);
        }

        public void Reset()
        {
        }

        #endregion
    }
}