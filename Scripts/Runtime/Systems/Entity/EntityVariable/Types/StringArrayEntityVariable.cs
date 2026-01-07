using D_Dev.ScriptableVaiables;

namespace D_Dev.EntityVariable.Types
{
    [System.Serializable]
    public class StringArrayEntityVariable : EntityVariable<string[]>
    {
        #region Constructor

        public StringArrayEntityVariable() { }
        public StringArrayEntityVariable(StringScriptableVariable id, string[] value) : base(id, value) { }

        #endregion

        #region Overrides

        public override BaseEntityVariable Clone()
        {
            return new StringArrayEntityVariable(_variableID, _value);
        }

        #endregion
    }
}
