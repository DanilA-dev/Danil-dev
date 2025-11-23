using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.PositionRotationConfig
{
    [System.Serializable]
    public class RotationConfig
    {
        #region Enums

        public enum RotationType
        {
            None = 0,
            EulerAngles = 1,
            Transform = 2
        }

        #endregion
        
        #region Fields

        [Title("Rotation")]
        [SerializeField] private RotationType _rotationType;
        [ShowIf(nameof(_rotationType), RotationType.EulerAngles)]
        [SerializeField] private Vector3 _eulerAngles;
        [ShowIf(nameof(_rotationType), RotationType.Transform)]
        [SerializeField] private bool _localRotTransform;
        [ShowIf(nameof(_rotationType), RotationType.Transform)]
        [SerializeField] private Transform _transformRot;

        #endregion

        #region Properties

        public RotationType Type
        {
            get => _rotationType;
            set => _rotationType = value;
        }

        public Vector3 EulerAngles
        {
            get => _eulerAngles;
            set => _eulerAngles = value;
        }

        public bool LocalRotTransform
        {
            get => _localRotTransform;
            set => _localRotTransform = value;
        }

        public Transform TransformRot
        {
            get => _transformRot;
            set => _transformRot = value;
        }

        #endregion

        #region Public

        public Quaternion GetRotation()
        {
            Quaternion rot = _rotationType switch
            {
                RotationType.None => Quaternion.identity,
                RotationType.EulerAngles => Quaternion.Euler(_eulerAngles),
                RotationType.Transform => _localRotTransform ? _transformRot.localRotation : _transformRot.rotation,
                _ => throw new ArgumentOutOfRangeException()
            };
            return rot;
        }

        public void SetRotation(ref Transform target)
        {
            target.rotation = GetRotation();
        }

        #endregion
    }
}
