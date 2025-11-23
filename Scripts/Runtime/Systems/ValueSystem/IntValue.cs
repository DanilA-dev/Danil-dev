using UnityEngine;

namespace D_Dev.ValueSystem
{
    [System.Serializable]
    public class IntValue : BaseValue<int>
    {
        public override int Value
        {
            get => _value;
            set
            {
                _value = (int)Mathf.Clamp(value, 0, Mathf.Infinity);
                OnValueChanged?.Invoke(_value);
            }
        }
    }
   
}
