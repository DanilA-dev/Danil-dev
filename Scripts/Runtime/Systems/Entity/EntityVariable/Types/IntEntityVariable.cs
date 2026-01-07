using D_Dev.ScriptableVaiables;

namespace D_Dev.EntityVariable.Types
{
    [System.Serializable]
    public class IntEntityVariable : EntityVariable<int>
    {
        #region Constructor

        public IntEntityVariable() { }
        public IntEntityVariable(StringScriptableVariable id,int value) : base(id,value) { }

        #endregion

        #region Overrides

        public override BaseEntityVariable Clone()
        {
            return new IntEntityVariable(_variableID, _value);
        }

        #endregion
    }
}