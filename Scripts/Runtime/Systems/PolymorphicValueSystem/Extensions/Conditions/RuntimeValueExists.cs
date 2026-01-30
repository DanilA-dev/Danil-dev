using D_Dev.Base;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem.Extensions
{
    [System.Serializable]
    public class RuntimeValueExists<T> : ICondition
    {
        #region Fields

        [SerializeReference] private PolymorphicValue<T> _value;
        [SerializeField] private bool _isExists;

        #endregion
        
        #region ICondition

        public bool IsConditionMet()
        {
            var result = _value.Value!= null;
            return result == _isExists;
        }

        public void Reset()
        {
        }

        #endregion
    }
}