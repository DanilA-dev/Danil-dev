using System;
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

        [SerializeField] private Transform _value;
        [SerializeField] private LocalDirection _direction;

        #endregion

        #region Overrides

        public override Vector3 GetPosition()
        {
            return _direction switch
            {
                LocalDirection.Up => _value.up,
                LocalDirection.Down => -_value.up,
                LocalDirection.Right => _value.right,
                LocalDirection.Left => -_value.right,
                LocalDirection.Forward => _value.forward,
                LocalDirection.Back => -_value.forward,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        #endregion
    }
}