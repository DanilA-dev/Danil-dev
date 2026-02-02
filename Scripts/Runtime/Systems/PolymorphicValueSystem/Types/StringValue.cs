using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class StringValue : PolymorphicValue<string> { }

    [System.Serializable]
    public sealed class StringConstantValue : ConstantValue<string>
    {
        #region Cloning

        public override PolymorphicValue<string> Clone()
        {
            return new StringConstantValue { _value = _value };
        }

        #endregion
    }

    [System.Serializable]
    public sealed class StringScriptableVariableValue : ScriptableVariableValue<StringScriptableVariable,string>
    {
        #region Cloning

        public override PolymorphicValue<string> Clone()
        {
            return new StringScriptableVariableValue { _variable = _variable };
        }

        #endregion
    }
}
