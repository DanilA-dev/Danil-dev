using System;
using UnityEngine;

namespace D_Dev.UtilScripts.ValueSystem
{
    public class BaseValue<T>
    {
        [SerializeField] protected T _value;

        public virtual T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
        public Action<T> OnValueChanged;
    }
}