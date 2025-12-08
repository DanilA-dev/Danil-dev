#if DOTWEEN
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.TweenAnimations.Types
{
    [System.Serializable]
    public class RotateAnimationTween : BaseAnimationTween
    {
        #region Enums

        public enum RotationMotionType
        {
            None = 0,
            Shake = 1,
            Punch = 2
        }

        #endregion

        #region Fields

        [SerializeField] private Transform _rotateObject;
        [SerializeField] private RotationMotionType _rotationMotionType;
        [ShowIf(nameof(_rotationMotionType), RotationMotionType.None)]
        [SerializeField] private RotateMode _rotateMode;
        [ShowIf(nameof(_rotationMotionType), RotationMotionType.None)]
        [SerializeField] private bool _useInitialRotationAsStart;
        [Space]
        [ShowIf(nameof(_rotationMotionType), RotationMotionType.None)]
        [SerializeField] private Vector3 _endValue;
        [ShowIf("@_rotationMotionType == RotationMotionType.None && !_useInitialRotationAsStart")]
        [SerializeField] private Vector3 _startValue;
        [ShowIf(nameof(_rotationMotionType), RotationMotionType.Shake)]
        [SerializeField] private Vector3 _shakeStrength = Vector3.one;
        [ShowIf(nameof(_rotationMotionType), RotationMotionType.Shake)]
        [SerializeField] private int _vibratoShake = 10;
        [ShowIf(nameof(_rotationMotionType), RotationMotionType.Shake)]
        [SerializeField] private float _randomnessShake = 90f;
        [ShowIf(nameof(_rotationMotionType), RotationMotionType.Shake)]
        [SerializeField] private bool _fadeOutShake = true;
        [ShowIf(nameof(_rotationMotionType), RotationMotionType.Punch)]
        [SerializeField] private Vector3 _punch = Vector3.one;
        [ShowIf(nameof(_rotationMotionType), RotationMotionType.Punch)]
        [SerializeField] private int _vibratoPunch = 10;
        [ShowIf(nameof(_rotationMotionType), RotationMotionType.Punch)]
        [SerializeField] private float _elasticityPunch = 1f;

        #endregion

        #region Properties

        public Transform RotateObject
        {
            get => _rotateObject;
            set => _rotateObject = value;
        }

        public RotationMotionType MotionType
        {
            get => _rotationMotionType;
            set => _rotationMotionType = value;
        }

        public RotateMode RotateMode
        {
            get => _rotateMode;
            set => _rotateMode = value;
        }

        public bool UseInitialRotationAsStart
        {
            get => _useInitialRotationAsStart;
            set => _useInitialRotationAsStart = value;
        }

        public Vector3 EndValue
        {
            get => _endValue;
            set => _endValue = value;
        }

        public Vector3 StartValue
        {
            get => _startValue;
            set => _startValue = value;
        }

        public Vector3 ShakeStrength
        {
            get => _shakeStrength;
            set => _shakeStrength = value;
        }

        public int VibratoShake
        {
            get => _vibratoShake;
            set => _vibratoShake = value;
        }

        public float RandomnessShake
        {
            get => _randomnessShake;
            set => _randomnessShake = value;
        }

        public bool FadeOutShake
        {
            get => _fadeOutShake;
            set => _fadeOutShake = value;
        }

        public Vector3 Punch
        {
            get => _punch;
            set => _punch = value;
        }

        public int VibratoPunch
        {
            get => _vibratoPunch;
            set => _vibratoPunch = value;
        }

        public float ElasticityPunch
        {
            get => _elasticityPunch;
            set => _elasticityPunch = value;
        }

        #endregion

        #region Override

        public override Tween Play()
        {
            switch (_rotationMotionType)
            {
                case RotationMotionType.None:
                    Tween = _rotateObject.DORotate(_endValue, Duration, _rotateMode).From(_useInitialRotationAsStart
                        ? _rotateObject.transform.eulerAngles
                        : _startValue);
                    break;
                case RotationMotionType.Shake:
                    Tween = _rotateObject.DOShakeRotation(Duration, _shakeStrength, _vibratoShake, _randomnessShake, _fadeOutShake);
                    break;
                case RotationMotionType.Punch:
                    Tween = _rotateObject.DOPunchRotation(_punch, Duration, _vibratoPunch, _elasticityPunch);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
            return Tween;
        }

        #endregion
    }
}
#endif
