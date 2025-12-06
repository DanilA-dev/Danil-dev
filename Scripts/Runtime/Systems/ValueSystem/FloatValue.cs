using D_Dev.ScriptableVaiables;
using D_Dev.ValueSystem.RandomMethods;
using UnityEngine;

namespace D_Dev.ValueSystem
{
    [System.Serializable]
    public class FloatValue : BaseValue<float, FloatScriptableVariable,FloatRandomValueMethod>
    {
        public override float Value
        {
            get => base.Value;
            set
            {
                _value = Mathf.Clamp(value, 0, Mathf.Infinity);
                OnValueChanged?.Invoke(_value);
            }
        }
    }
    
}
