using UnityEngine;

namespace D_Dev.UtilScripts.ValueSystem
{
    [System.Serializable]
    public class FloatValue : BaseValue<float>
    {
        public override float Value
        {
            get => _value;
            set
            {
                _value = Mathf.Clamp(value, 0, Mathf.Infinity);
                OnValueChanged?.Invoke(_value);
            }
        }
    }
    
}