using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PositionSetter
{
    public enum PositionType
    {
        Vector = 0,
        Transform = 1,
        Random_Vector = 2,
        Random_Transform = 3,
    }

    public enum RotationType
    {
        None = 0,
        EulerAngles = 1,
        Transform = 2
    }
    
    public enum PositionRandomizeType
    {
        None = 0,
        Random_In_Sphere = 1,
        Random_In_Circle = 2
    }
    
    [System.Serializable]
    public class PositionConfig
    {
        [Title("Position")]
        [SerializeField] private PositionType _positionType;
        [ShowIf(nameof(_positionType), PositionType.Vector)]
        [SerializeField] private Vector3 _vectorPos;
        [ShowIf(nameof(_positionType), PositionType.Transform)]
        [SerializeField] private bool _localPosTransform;
        [ShowIf(nameof(_positionType), PositionType.Transform)]
        [SerializeField] private Transform _transformPos;
        [ShowIf(nameof(_positionType), PositionType.Random_Vector)]
        [SerializeField] private Vector3[] _randomVectorPos;
        [ShowIf(nameof(_positionType), PositionType.Random_Transform)]
        [SerializeField] private Transform[] _randomTransforms;
        [SerializeField] private PositionRandomizeType _positionRandomizeType;
        [HideIf(nameof(_positionRandomizeType), PositionRandomizeType.None)]
        [SerializeField] private float _randomRadius;
        [Space]
        [Title("Rotation")] 
        [SerializeField] private RotationType _rotationType;
        [ShowIf(nameof(_rotationType), RotationType.EulerAngles)]
        [SerializeField] private Vector3 _eulerAngles;
        [ShowIf(nameof(_rotationType), RotationType.Transform)]
        [SerializeField] private bool _localRotTransform;
        [ShowIf(nameof(_rotationType), RotationType.Transform)]
        [SerializeField] private Transform _transformRot;
        
        
        public Vector3 GetPosition()
        {
            Vector3 pos = _positionType switch
            {
                PositionType.Vector => _vectorPos,
                PositionType.Transform => _localPosTransform ? _transformPos.localPosition : _transformPos.position,
                PositionType.Random_Vector => _randomVectorPos[Random.Range(0, _randomVectorPos.Length)],
                PositionType.Random_Transform => _randomTransforms[Random.Range(0, _randomTransforms.Length)].position,
                _ => Vector3.zero
            };

            return _positionRandomizeType switch
            {
                PositionRandomizeType.None => pos,
                PositionRandomizeType.Random_In_Sphere => pos += Random.insideUnitSphere * _randomRadius,
                PositionRandomizeType.Random_In_Circle => pos += new Vector3(Random.insideUnitCircle.x * _randomRadius,
                    Random.insideUnitCircle.y * _randomRadius, pos.z),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

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
    }
}