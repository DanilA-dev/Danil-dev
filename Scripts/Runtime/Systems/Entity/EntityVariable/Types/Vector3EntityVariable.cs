using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.EntityVariable.Types
{
    [System.Serializable]
    public class Vector3EntityVariable : EntityVariable<Vector3>
    {
        #region Constructor

        public Vector3EntityVariable() { }
        public Vector3EntityVariable(StringScriptableVariable id, Vector3 value) : base(id, value) { }

        #endregion

        #region Overrides

        public override BaseEntityVariable Clone()
        {
            return new Vector3EntityVariable(_variableID, _value);
        }

        #endregion
    }
}
