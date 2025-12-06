using System;
using D_Dev.ValueSystem;
using UnityEngine;

namespace D_Dev.PositionRotationConfig.RotationSettings
{
    [Serializable]
    public class TransformRotationSettings : BaseRotationSettings
    {
        #region Fields

        [SerializeField] private TransformValue _value;
        [SerializeField] private bool _isLocal;

        #endregion

        #region Overrides

        public override Quaternion GetRotation() => _isLocal 
            ? _value.Value.localRotation 
            : _value.Value.rotation;

        #endregion
    }
}