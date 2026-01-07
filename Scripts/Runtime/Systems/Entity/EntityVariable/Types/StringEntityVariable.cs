using D_Dev.ScriptableVaiables;

namespace D_Dev.EntityVariable.Types
{
    [System.Serializable]
    public class StringEntityVariable : EntityVariable<string>
    {
        #region Constructor

        public StringEntityVariable() { }
        public StringEntityVariable(StringScriptableVariable id, string value) : base(id, value) { }

        #endregion

        #region Overrides

        public override BaseEntityVariable Clone()
        {
            return new StringEntityVariable(_variableID, _value);
        }

        #endregion
    }
}
