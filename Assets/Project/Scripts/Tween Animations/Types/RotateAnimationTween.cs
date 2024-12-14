#if DOTWEEN
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Tween_Animations.Types
{
    [System.Serializable]
    public class RotateAnimationTween : BaseAnimationTween
    {
        [SerializeField] private Transform _rotateObject;
        [SerializeField] private RotateMode _rotateMode;
        [SerializeField] private bool _useInitialRotationAsStart;
        [Space]
        [SerializeField] private Vector3 _endValue;
        [HideIf(nameof(_useInitialRotationAsStart))]
        [SerializeField] private Vector3 _startValue;

        public Transform RotateObject
        {
            get => _rotateObject;
            set => _rotateObject = value;
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

        public override Tween Play()
        {
            Tween = _rotateObject.DORotate(_endValue, Duration, _rotateMode).From(_useInitialRotationAsStart 
                ? _rotateObject.transform.eulerAngles
                : _startValue);
            return Tween;
        }
    }
}
#endif
