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
    public class ColorTween : BaseAnimationTween
    {
        private enum ColorObject
        {
            Image,
            Material,
            SpriteRenderer
        }
        
        [SerializeField] private ColorObject _colorObject;
        [ShowIf("@this._colorObject == ColorObject.Image")]
        [SerializeField] private Image _image;
        [FormerlySerializedAs("_material")]
        [ShowIf("@this._colorObject == ColorObject.Material")]
        [SerializeField] private MeshRenderer _meshMaterial;
        [ShowIf("@this._colorObject == ColorObject.SpriteRenderer")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private bool _useInitilColorAsStart;
        [HideIf(nameof(_useInitilColorAsStart))]
        [SerializeField] private Color _startValue;
        [SerializeField] private Color _endValue;
        
        public override Tween Play()
        {
            switch (_colorObject)
            {
                case ColorObject.Image:
                {
                    var startColor = _useInitilColorAsStart ? _image.color : _startValue;
                    return Tween = _image.DOColor(_endValue, _duration).From(startColor);
                }
                case ColorObject.Material:
                {
                    var material = _meshMaterial.sharedMaterial;
                    var startColor = _useInitilColorAsStart ? material.color : _startValue;
                    return Tween = material.DOColor(_endValue, _duration).From(startColor);
                }
                case ColorObject.SpriteRenderer:
                {
                    var startColor = _useInitilColorAsStart ? _spriteRenderer.color : _startValue;
                    return Tween = _spriteRenderer.DOColor(_endValue, _duration).From(startColor);
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
#endif
