using D_Dev.CustomEventManager;
using D_Dev.PolymorphicValueSystem;
using D_Dev.PositionRotationConfig;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.CurrencySystem.Extensions
{
    public class EntityFlyingAnimationInvoker : MonoBehaviour
    {
        #region Fields

        [SerializeReference] private PolymorphicValue<string> _eventName = new StringConstantValue();
        [SerializeReference] private BasePositionSettings _from = new();
        [SerializeReference] private BasePositionSettings _to = new();
        [SerializeReference] private PolymorphicValue<int> _amount = new IntConstantValue();

        #endregion

        #region Public

        [Button]
        public void Invoke()
        {
            EventManager.Invoke(_eventName.Value,_from.GetPosition(), _to.GetPosition(), _amount.Value);
        }

        #endregion
    }
}