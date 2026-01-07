using D_Dev.ScriptableVaiables;
using UnityEngine;

namespace D_Dev.EntityVariable.Types
{
    [System.Serializable]
    public class Vector2EntityVariable : EntityVariable<Vector2>
    {
        #region Constructor

        public Vector2EntityVariable() { }
        public Vector2EntityVariable(StringScriptableVariable id, Vector2 value) : base(id, value) { }

        #endregion

        #region Overrides

        public override BaseEntityVariable Clone()
        {
            return new Vector2EntityVariable(_variableID, _value);
        }

        #endregion
    }
}
