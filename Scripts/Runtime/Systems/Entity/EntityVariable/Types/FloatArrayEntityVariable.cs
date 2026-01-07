using D_Dev.ScriptableVaiables;

namespace D_Dev.EntityVariable.Types
{
    [System.Serializable]
    public class FloatArrayEntityVariable : EntityVariable<float[]>
    {
        #region Constructor

        public FloatArrayEntityVariable() { }
        public FloatArrayEntityVariable(StringScriptableVariable id, float[] value) : base(id, value) { }

        #endregion

        #region Overrides

        public override BaseEntityVariable Clone()
        {
            return new FloatArrayEntityVariable(_variableID, _value);
        }

        #endregion
    }
}
