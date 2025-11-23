using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace D_Dev.PositionRotationConfig
{
    [System.Serializable]
    public class PositionConfig
    {
        #region Enums

        public enum PositionType
        {
            Vector = 0,
            Transform = 1,
            TransformDirection = 2,
            RandomVector = 3,
            RandomTransform = 4,
        }

        public enum LocalTransformDirection
        {
            Self = 0,
            Up = 1,
            Down = 2,
            Right = 3,
            Left = 4,
            Forward = 5,
            Back = 7
        }

        public enum PositionRandomizeType
        {
            None = 0,
            RandomInSphere = 1,
            RandomInCircle = 2
        }
        #endregion

        #region Fields

        [Title("Position")]
        [SerializeField] private PositionType _positionType;
        [ShowIf(nameof(_positionType), PositionType.Vector)]
        [SerializeField] private Vector3 _vectorPos;
        [ShowIf(nameof(_positionType), PositionType.Transform)]
        [SerializeField] private bool _local;
        [ShowIf("@_positionType == PositionType.TransformDirection || _positionType == PositionType.Transform")]
        [SerializeField] private Transform _transform;
        [ShowIf(nameof(_positionType), PositionType.TransformDirection)]
        [SerializeField] private LocalTransformDirection _transformDirection;
        [ShowIf(nameof(_positionType), PositionType.RandomVector)]
        [SerializeField] private Vector3[] _randomVectorPos;
        [ShowIf(nameof(_positionType), PositionType.RandomTransform)]
        [SerializeField] private Transform[] _randomTransforms;
        [SerializeField] private PositionRandomizeType _positionRandomizeType;
        [HideIf(nameof(_positionRandomizeType), PositionRandomizeType.None)]
        [SerializeField] private float _randomRadius;
        [SerializeField] private bool _setParent;
        [SerializeField, ShowIf(nameof(_setParent))] private Transform _parent;

        #endregion

        #region Properties

        public PositionType Type
        {
            get => _positionType;
            set => _positionType = value;
        }

        public Transform Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public Vector3 VectorPos
        {
            get => _vectorPos;
            set => _vectorPos = value;
        }

        public bool LocalPosTransform
        {
            get => _local;
            set => _local = value;
        }

        public Transform Transform
        {
            get => _transform;
            set => _transform = value;
        }

        public LocalTransformDirection TransformDirection
        {
            get => _transformDirection;
            set => _transformDirection = value;
        }

        public Vector3[] RandomVectorPos
        {
            get => _randomVectorPos;
            set => _randomVectorPos = value;
        }

        public Transform[] RandomTransforms
        {
            get => _randomTransforms;
            set => _randomTransforms = value;
        }

        public PositionRandomizeType RandomizeType
        {
            get => _positionRandomizeType;
            set => _positionRandomizeType = value;
        }

        public float RandomRadius
        {
            get => _randomRadius;
            set => _randomRadius = value;
        }

        #endregion

        #region Public

        public Vector3 GetPosition()
        {
            Vector3 pos = _positionType switch
            {
                PositionType.Vector => _vectorPos,
                PositionType.Transform => _local ? _transform.localPosition : _transform.position,
                PositionType.TransformDirection => _transformDirection switch
                {
                    LocalTransformDirection.Self => _transform.position,
                    LocalTransformDirection.Up => _transform.up,
                    LocalTransformDirection.Down => -_transform.up,
                    LocalTransformDirection.Right => _transform.right,
                    LocalTransformDirection.Left => -_transform.right,
                    LocalTransformDirection.Forward => _transform.forward,
                    LocalTransformDirection.Back => -_transform.forward,
                    _ => throw new ArgumentOutOfRangeException()
                },
                PositionType.RandomVector => _randomVectorPos[Random.Range(0, _randomVectorPos.Length)],
                PositionType.RandomTransform => _randomTransforms[Random.Range(0, _randomTransforms.Length)].position,
                _ => Vector3.zero
            };

            return _positionRandomizeType switch
            {
                PositionRandomizeType.None => pos,
                PositionRandomizeType.RandomInSphere => pos += Random.insideUnitSphere * _randomRadius,
                PositionRandomizeType.RandomInCircle => pos += new Vector3(Random.insideUnitCircle.x * _randomRadius,
                    Random.insideUnitCircle.y * _randomRadius, pos.z),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void SetPosition(ref Transform target)
        {
            if(_setParent)
                target.parent = _parent;
            
            target.position = GetPosition();
        }

        #endregion
    }
}
