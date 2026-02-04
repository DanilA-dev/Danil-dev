using System;
using D_Dev.PolymorphicValueSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace D_Dev.PositionRotationConfig
{
    [Serializable]
    public class RandomAroundTransformPositionSettings : BasePositionSettings
    {
        #region Fields

        [SerializeReference] private PolymorphicValue<Transform> _target;
        [SerializeField] private float _radius = 5f;
        [SerializeField] private bool _useLocalPosition = false;

        #endregion

        #region Properties

        public PolymorphicValue<Transform> Target
        {
            get => _target;
            set => _target = value;
        }

        public float Radius
        {
            get => _radius;
            set => _radius = value;
        }

        public bool UseLocalPosition
        {
            get => _useLocalPosition;
            set => _useLocalPosition = value;
        }

        #endregion

        #region Overrides

        public override Vector3 GetPosition()
        {
            if (_target?.Value == null)
            {
                Debug.Log($"[PositionSettings] Value is null, reset to Vector.zero");
                return Vector3.zero;
            }

            Vector3 center = _useLocalPosition 
                ? _target.Value.localPosition 
                : _target.Value.position;

            Vector3 randomOffset = Random.insideUnitSphere * _radius;
            randomOffset.y = 0;

            return center + randomOffset;
        }

        #endregion
    }
}
