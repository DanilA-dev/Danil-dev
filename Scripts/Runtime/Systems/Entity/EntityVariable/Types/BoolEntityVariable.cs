using D_Dev.ScriptableVaiables;

namespace D_Dev.EntityVariable.Types
{
    [System.Serializable]
    public class BoolEntityVariable : EntityVariable<bool>
    {
        #region Constructor

        public BoolEntityVariable() { }
        public BoolEntityVariable(StringScriptableVariable id, bool value) : base(id, value) { }

        #endregion

        #region Overrides

        public override BaseEntityVariable Clone()
        {
            return new BoolEntityVariable(_variableID, _value);
        }

        #endregion
    }
}
