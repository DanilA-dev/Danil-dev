using D_Dev.ScriptableVaiables;
using D_Dev.ValueSystem.RandomMethods;

namespace D_Dev.ValueSystem
{
    [System.Serializable]
    public class StringValue : BaseValue<string, StringScriptableVariable,ArrayRandomValueMethod<string>> { }
}