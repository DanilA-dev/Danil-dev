using System;
using D_Dev.ValueSystem;
using UnityEngine;

namespace D_Dev.PositionRotationConfig
{
    [Serializable]
    public class TransformPositionSettings : BasePositionSettings
    {
        #region Fields

        [SerializeField] private TransformValue _value;
        [SerializeField] private bool _isLocal;

        #endregion

        #region Overrides

        public override Vector3 GetPosition() => _isLocal ?
            _value.Value.localPosition 
            : _value.Value.position;

        #endregion
    }
}