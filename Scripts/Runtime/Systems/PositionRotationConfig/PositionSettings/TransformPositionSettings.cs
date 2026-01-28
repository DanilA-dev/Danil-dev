using System;
using D_Dev.PolymorphicValueSystem;
using UnityEngine;

namespace D_Dev.PositionRotationConfig
{
    [Serializable]
    public class TransformPositionSettings : BasePositionSettings
    {
        #region Fields

        [SerializeReference] private PolymorphicValue<Transform> _value;
        [SerializeField] private bool _isLocal;

        #endregion

        #region Properties

        public PolymorphicValue<Transform> Value
        {
            get => _value;
            set => _value = value;
        }

        public bool IsLocal
        {
            get => _isLocal;
            set => _isLocal = value;
        }

        #endregion
        
        #region Overrides

        public override Vector3 GetPosition() => _isLocal ?
            _value.Value.localPosition 
            : _value.Value.position;

        #endregion
    }
}