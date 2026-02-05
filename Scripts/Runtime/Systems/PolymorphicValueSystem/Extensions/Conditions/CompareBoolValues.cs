using D_Dev.Base;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem.Extensions
{
    [System.Serializable]
    public class CompareBoolValues : ICondition
    {
        #region Fields

        [SerializeReference] private PolymorphicValue<bool> _valueA;
        [SerializeField] private bool _compareType;
        [SerializeReference] private PolymorphicValue<bool> _valueB;
        
        #endregion

        #region IConditions

        public bool IsConditionMet()
        {
            if(_compareType)
                return _valueA.Value == _valueB.Value;
            
            return _valueA.Value != _valueB.Value;
        }

        public void Reset() {}

        #endregion
    }
}