using System;
using UnityEngine;

namespace D_Dev.ValueSystem
{
    public class BaseValue<T>
    {
        #region Fields

        [SerializeField] protected T _value;
        

        public Action<T> OnValueChanged;
        #endregion

        #region Properties

        public virtual T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }

        #endregion

       
    }
}
