using D_Dev.PolymorphicValueSystem;
using UnityEngine;

namespace D_Dev.PositionRotationConfig
{
    [System.Serializable]
    public class Vector3PositionSettings : BasePositionSettings
    {
        #region Fields

        [SerializeReference] private PolymorphicValue<Vector3> _value;

        #endregion

        #region Overrides
        
        public override Vector3 GetPosition() => _value.Value;

        #endregion
    }
}