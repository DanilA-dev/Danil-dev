using D_Dev.ScriptableVaiables;

namespace D_Dev.EntityVariable.Types
{
    [System.Serializable]
    public class FloatEntityVariable : EntityVariable<float>
    {
        #region Constructors

        public FloatEntityVariable() {}
        public FloatEntityVariable(StringScriptableVariable id,float value) : base(id,value) {}


        #endregion

        #region Overrides

        public override BaseEntityVariable Clone()
        {
            return new FloatEntityVariable(_variableID,_value);
        }

        #endregion
    }
}