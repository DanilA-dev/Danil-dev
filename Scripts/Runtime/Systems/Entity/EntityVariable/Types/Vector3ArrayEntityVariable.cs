using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.EntityVariable.Types
{
    [System.Serializable]
    public class Vector3ArrayEntityVariable : EntityVariable<Vector3[]>
    {
        #region Constructor

        public Vector3ArrayEntityVariable() { }
        public Vector3ArrayEntityVariable(StringScriptableVariable id, Vector3[] value) : base(id, value) { }

        #endregion

        #region Overrides

        public override BaseEntityVariable Clone()
        {
            return new Vector3ArrayEntityVariable(_variableID, _value);
        }

        #endregion
    }
}
