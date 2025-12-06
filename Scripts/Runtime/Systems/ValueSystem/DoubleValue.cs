using D_Dev.ScriptableVaiables;
using D_Dev.ValueSystem.RandomMethods;
using UnityEngine;

namespace D_Dev.ValueSystem
{
    [System.Serializable]
    public class DoubleValue : BaseValue<double, DoubleScriptableVariable,DoubleRandomValueMethod>
    {
        public override double Value
        {
            get => base.Value;
            set
            {
                _value = Mathf.Clamp((float)value, 0, Mathf.Infinity);
                OnValueChanged?.Invoke(_value);
            }
        }
    }
}
