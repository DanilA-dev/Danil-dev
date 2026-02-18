#if DOTWEEN
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.TweenAnimations.Types
{
    [System.Serializable]
    public class RotateAnimationTween : BaseAnimationTween
    {
        #region Fields

        [SerializeField] private Transform _rotateObject;
        [SerializeField] private MotionType _motionType;
        [ShowIf(nameof(_motionType), MotionType.None)]
        [SerializeField] private RotateMode _rotateMode;
        [ShowIf(nameof(_motionType), MotionType.None)]
        [SerializeField] private bool _useInitialRotationAsStart;
        [Space]
        [ShowIf(nameof(_motionType), MotionType.None)]
        [SerializeField] private Vector3 _endValue;
        [ShowIf("@_motionType == MotionType.None && !_useInitialRotationAsStart")]
        [SerializeField] private Vector3 _startValue;
        [ShowIf(nameof(_motionType), MotionType.Shake)]
        [SerializeField] private Vector3 _shakeStrength = Vector3.one;
        [ShowIf(nameof(_motionType), MotionType.Shake)]
        [SerializeField] private int _vibratoShake = 10;
        [ShowIf(nameof(_motionType), MotionType.Shake)]
        [SerializeField] private float _randomnessShake = 90f;
        [ShowIf(nameof(_motionType), MotionType.Shake)]
        [SerializeField] private bool _fadeOutShake = true;
        [ShowIf(nameof(_motionType), MotionType.Punch)]
        [SerializeField] private Vector3 _punch = Vector3.one;
        [ShowIf(nameof(_motionType), MotionType.Punch)]
        [SerializeField] private int _vibratoPunch = 10;
        [ShowIf(nameof(_motionType), MotionType.Punch)]
        [SerializeField] private float _elasticityPunch = 1f;

        #endregion

        #region Properties

        public Transform RotateObject
        {
            get => _rotateObject;
            set => _rotateObject = value;
        }

        public MotionType Motion
        {
            get => _motionType;
            set => _motionType = value;
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
            if (_rotateObject == null)
                return null;
            
            SetTarget(_rotateObject.gameObject);
            
            switch (_motionType)
            {
                case MotionType.None:
                    Tween = _rotateObject.DORotate(_endValue, Duration, _rotateMode).From(_useInitialRotationAsStart
                        ? _rotateObject.transform.eulerAngles
                        : _startValue);
                    break;
                case MotionType.Shake:
                    Tween = _rotateObject.DOShakeRotation(Duration, _shakeStrength, _vibratoShake, _randomnessShake, _fadeOutShake);
                    break;
                case MotionType.Punch:
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
