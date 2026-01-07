using D_Dev.ScriptableVaiables;

namespace D_Dev.EntityVariable.Types
{
    [System.Serializable]
    public class DoubleEntityVariable : EntityVariable<double>
    {
        #region Constructor

        public DoubleEntityVariable() { }
        public DoubleEntityVariable(StringScriptableVariable id, double value) : base(id, value) { }

        #endregion

        #region Overrides

        public override BaseEntityVariable Clone()
        {
            return new DoubleEntityVariable(_variableID, _value);
        }

        #endregion
    }
}
