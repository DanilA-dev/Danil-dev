using System;
using D_Dev.ScriptableVaiables;
using D_Dev.ValueSystem.RandomMethods;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.ValueSystem
{
    #region Enums

    public enum ValueType
    {
        Constant = 0,
        Random = 1,
        Variable = 2,
    }

    #endregion
    
    [Serializable]
    public class BaseValue<T,TVariable,TRandom>  where TVariable : BaseScriptableVariable<T>
        where TRandom : BaseRandomValueMethod<T>
    {
        #region Fields

        [SerializeField] protected ValueType _valueType;
        [ShowIf("_valueType", ValueType.Constant)]
        [SerializeField] protected T _value;

        [ShowIf("_valueType", ValueType.Random)] 
        [SerializeField] protected TRandom _randomValue;
        [ShowIf("_valueType", ValueType.Variable)]
        [SerializeField] protected TVariable _variable;
        
        public Action<T> OnValueChanged;
        
        #endregion

        #region Properties

        public virtual T Value
        {
            get
            {
                return _valueType switch
                {
                    ValueType.Constant => _value,
                    ValueType.Random => _randomValue.GetRandomValue(),
                    ValueType.Variable => _variable.Value,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            set
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }

        protected ValueType ValueType
        {
            get => _valueType;
            set => _valueType = value;
        }

        #endregion
    }
}
