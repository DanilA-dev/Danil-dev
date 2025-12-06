using System;
using D_Dev.ValueSystem;
using UnityEngine;

namespace D_Dev.PositionRotationConfig.RotationSettings
{
    [Serializable]
    public class EulerAnglesRotationSettings : BaseRotationSettings
    {
        #region Fields

        [SerializeField] private Vector3Value _value;

        #endregion

        #region Overrides

        public override Quaternion GetRotation() => Quaternion.Euler(_value.Value);

        #endregion
    }
}