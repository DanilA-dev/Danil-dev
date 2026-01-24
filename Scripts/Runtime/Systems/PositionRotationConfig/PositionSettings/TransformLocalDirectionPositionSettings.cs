using System;
using D_Dev.PolymorphicValueSystem;
using UnityEngine;

namespace D_Dev.PositionRotationConfig
{
    [Serializable]
    public class TransformLocalDirectionPositionSettings : BasePositionSettings
    {
        #region Enums

        public enum LocalDirection
        {
            Up = 0,
            Down = 1,
            Right = 2,
            Left = 3,
            Forward = 4,
            Back = 5
        }

        #endregion
        
        #region Fields

        [SerializeReference] private PolymorphicValue<Transform> _value;
        [SerializeField] private LocalDirection _direction;

        #endregion

        #region Overrides

        public override Vector3 GetPosition()
        {
            return _direction switch
            {
                LocalDirection.Up => _value.Value.up,
                LocalDirection.Down => -_value.Value.up,
                LocalDirection.Right => _value.Value.right,
                LocalDirection.Left => -_value.Value.right,
                LocalDirection.Forward => _value.Value.forward,
                LocalDirection.Back => -_value.Value.forward,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        #endregion
    }
}