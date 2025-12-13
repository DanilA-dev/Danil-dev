using System;
using UnityEngine;

namespace D_Dev.PositionRotationConfig
{
    [Serializable]
    public class TransformPositionSettings : BasePositionSettings
    {
        #region Fields

        [SerializeField] private Transform _value;
        [SerializeField] private bool _isLocal;

        #endregion

        #region Overrides

        public override Vector3 GetPosition() => _isLocal ?
            _value.localPosition 
            : _value.position;

        #endregion
    }
}