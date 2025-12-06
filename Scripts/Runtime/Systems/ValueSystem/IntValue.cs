using D_Dev.ScriptableVaiables;
using D_Dev.ValueSystem.RandomMethods;
using UnityEngine;

namespace D_Dev.ValueSystem
{
    [System.Serializable]
    public class IntValue : BaseValue<int, IntScriptableVariable,IntRandomValueMethod>
    {
        public override int Value
        {
            get => base.Value;
            set
            {
                _value = (int)Mathf.Clamp(value, 0, Mathf.Infinity);
                OnValueChanged?.Invoke(_value);
            }
        }
    }
   
}
