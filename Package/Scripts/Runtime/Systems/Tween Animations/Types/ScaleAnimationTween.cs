#if DOTWEEN
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.TweenAnimations.Types
{
    [System.Serializable]
    public class ScaleAnimationTween : BaseAnimationTween
    {
        #region Fields

        [SerializeField] private Transform[] _scaleObjects;
        [SerializeField] private MotionType _motionType;
        [ShowIf(nameof(_motionType), MotionType.None)]
        [SerializeField] private bool _useInitialScaleAsStart;
        [ShowIf(nameof(_motionType), MotionType.None)]
        [SerializeField] private Vector3 _endScale;
        [ShowIf("@!_useInitialScaleAsStart")]
        [SerializeField] private Vector3 _startScale;
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

        public Transform[] ScaleObjects
        {
            get => _scaleObjects;
            set => _scaleObjects = value;
        }

        public MotionType Motion
        {
            get => _motionType;
            set => _motionType = value;
        }

        public bool UseInitialScaleAsStart
        {
            get => _useInitialScaleAsStart;
            set => _useInitialScaleAsStart = value;
        }

        public Vector3 EndScale
        {
            get => _endScale;
            set => _endScale = value;
        }

        public Vector3 StartScale
        {
            get => _startScale;
            set => _startScale = value;
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
            if (_scaleObjects == null || _scaleObjects.Length == 0)
                return null;

            Sequence sequence = DOTween.Sequence();
            
            foreach (var scaleObject in _scaleObjects)
            {
                if (scaleObject == null)
                    continue;

                Tween objectTween = null;
                switch (_motionType)
                {
                    case MotionType.None:
                        objectTween = scaleObject.DOScale(_endScale, Duration)
                            .From(_useInitialScaleAsStart
                                ? scaleObject.transform.localScale
                                : _startScale);
                        break;
                    case MotionType.Shake:
                        objectTween = scaleObject.DOShakeScale(Duration, _shakeStrength, _vibratoShake, _randomnessShake, _fadeOutShake)
                            .OnStart(() => scaleObject.localScale = _startScale);
                        break;
                    case MotionType.Punch:
                        objectTween = scaleObject.DOPunchScale(_punch, Duration, _vibratoPunch, _elasticityPunch)
                            .OnStart(() => scaleObject.localScale = _startScale);
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException();
                }
                
                if (objectTween != null)
                    sequence.Join(objectTween);
            }
            
            SetTarget(_scaleObjects[0]?.gameObject);
            Tween = sequence;
            return Tween;
        }

        public override void Pause()
        {
            Tween.Pause();
        }

        #endregion
    }
}
#endif
