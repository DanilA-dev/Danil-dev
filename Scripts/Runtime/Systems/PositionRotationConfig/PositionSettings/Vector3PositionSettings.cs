using D_Dev.ValueSystem;
using UnityEngine;

namespace D_Dev.PositionRotationConfig
{
    [System.Serializable]
    public class Vector3PositionSettings : BasePositionSettings
    {
        #region Fields

        [SerializeField] private Vector3Value _value;

        #endregion

        #region Overrides

        public override Vector3 GetPosition() => _value.Value;

        #endregion
    }
}