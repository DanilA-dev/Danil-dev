using UnityEngine;

namespace D_Dev.ValueSystem
{
    public class DoubleValue : BaseValue<double>
    {
        public override double Value
        {
            get => _value;
            set
            {
                _value = Mathf.Clamp((float)value, 0, Mathf.Infinity);
                OnValueChanged?.Invoke(_value);
            }
        }
    }
}
