#if DOTWEEN
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Tween_Animations.Types
{
    [System.Serializable]
    public class ScaleAnimationTween : BaseAnimationTween
    {
        [SerializeField] private Transform _scaleObject;
        [SerializeField] private bool _useInitialScaleAsStart;
        [SerializeField] private Vector3 _endScale;
        [HideIf(nameof(_useInitialScaleAsStart))]
        [SerializeField] private Vector3 _startScale;

        public Transform ScaleObject
        {
            get => _scaleObject;
            set => _scaleObject = value;
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

        public override Tween Play()
        {
            Tween = _scaleObject.DOScale(_endScale, Duration)
                .From(_useInitialScaleAsStart
                    ? _scaleObject.transform.localScale
                    : _startScale);
            
            return Tween;
        }

        public override void Pause()
        {
            Tween.Pause();
        }
    }
}
#endif
