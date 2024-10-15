#if DOTWEEN
using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TweenAnimations
{
    [Serializable]
    public class FadeTween : BaseAnimationTween
    {
        private enum FadeObject
        {
            Image,
            CanvasGroup,
            Material,
            SpriteRenderer
        }
        
        [SerializeField] private FadeObject _fadeObject;
        [ShowIf("@this._fadeObject == FadeObject.Image")]
        [SerializeField] private Image _image;
        [FormerlySerializedAs("_material")]
        [ShowIf("@this._fadeObject == FadeObject.Material")]
        [SerializeField] private MeshRenderer _meshMaterial;
        [ShowIf("@this._fadeObject == FadeObject.CanvasGroup")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [ShowIf("@this._fadeObject == FadeObject.SpriteRenderer")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _startValue;
        [SerializeField] private float _endValue;
        
        public override Tween Play()
        {
            switch (_fadeObject)
            {
                case FadeObject.Image:
                    return Tween = _image.DOFade(_endValue, _duration).From(_startValue);
                case FadeObject.CanvasGroup:
                    return Tween = _canvasGroup.DOFade(_endValue, _duration).From(_startValue);
                case FadeObject.Material:
                {
                    var material = _meshMaterial.sharedMaterial;
                    return Tween = material.DOFade(_endValue, _duration).From(_startValue);
                }
                case FadeObject.SpriteRenderer:
                    return Tween = _spriteRenderer.DOFade(_endValue, _duration).From(_startValue);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
#endif
