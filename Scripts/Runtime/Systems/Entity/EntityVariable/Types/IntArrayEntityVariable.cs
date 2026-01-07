using D_Dev.ScriptableVaiables;

namespace D_Dev.EntityVariable.Types
{
    [System.Serializable]
    public class IntArrayEntityVariable : EntityVariable<int[]>
    {
        #region Constructor

        public IntArrayEntityVariable() { }
        public IntArrayEntityVariable(StringScriptableVariable id, int[] value) : base(id, value) { }

        #endregion

        #region Overrides

        public override BaseEntityVariable Clone()
        {
            return new IntArrayEntityVariable(_variableID, _value);
        }

        #endregion
    }
}
