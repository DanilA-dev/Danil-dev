#if DOTWEEN
using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace D_Dev.FocusScroll
{
    [RequireComponent(typeof(ScrollRect))]
    public class CenterFocusScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        #region Fields

        [Title("Layout")]
        [SerializeField] private bool _autoPadding = true;
        [Title("Focus")]
        [SerializeField] private AnimationCurve _scaleByDistance = new(new Keyframe(0, 1.25f), new Keyframe(1, 0.85f), new Keyframe(3, 0.6f));
        [SerializeField] private bool _fadeByDistance;
        [ShowIf(nameof(_fadeByDistance))]
        [SerializeField] private AnimationCurve _alphaByDistance = new(new Keyframe(0, 1f), new Keyframe(3, 0.4f));
        [Title("Snap")]
        [SerializeField] private float _snapDuration = 0.3f;
        [SerializeField] private Ease _snapEase = Ease.OutCubic;
        [SerializeField] private float _inertiaProjection = 0.15f;
        [SerializeField] private bool _ignoreTimeScale = true;
        [SerializeField] private bool _scrollOnClick = true;

        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent<int> _onCenteredChanged;

        private readonly List<RectTransform> _items = new();
        private readonly List<RectTransform> _collectBuffer = new();
        private readonly List<CanvasGroup> _canvasGroups = new();
        private ScrollRect _scrollRect;
        private RectTransform _content;
        private RectTransform _viewport;
        private HorizontalOrVerticalLayoutGroup _layoutGroup;
        private Tween _snapTween;
        private int _centeredIndex = -1;
        private bool _isPaddingDirty = true;

        public event Action<int> OnCenteredChanged;

        #endregion

        #region Properties

        public int CenteredIndex => _centeredIndex;
        public IReadOnlyList<RectTransform> Items => _items;

        private bool IsHorizontal => _scrollRect.horizontal;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _content = _scrollRect.content;
            _viewport = _scrollRect.viewport != null ? _scrollRect.viewport : (RectTransform)transform;
            _layoutGroup = _content.GetComponent<HorizontalOrVerticalLayoutGroup>();
        }

        private void OnDisable()
        {
            _snapTween?.Kill();
            _snapTween = null;
        }

        private void LateUpdate()
        {
            CollectItems();
            UpdatePadding();
            UpdateFocus();
        }

        private void OnRectTransformDimensionsChange() => _isPaddingDirty = true;

        #endregion

        #region Public

        public void ScrollTo(int index, bool instant = false)
        {
            CollectItems();
            UpdatePadding();

            if (index < 0 || index >= _items.Count)
                return;

            _snapTween?.Kill();
            _scrollRect.StopMovement();

            if (instant)
                LayoutRebuilder.ForceRebuildLayoutImmediate(_content);

            var delta = GetAxis(_viewport.rect.center) - GetAxis(GetItemCenter(_items[index]));
            var target = _content.anchoredPosition;
            if (IsHorizontal)
                target.x += delta;
            else
                target.y += delta;

            if (instant)
            {
                _content.anchoredPosition = target;
                UpdateFocus();
                return;
            }

            _snapTween = _content.DOAnchorPos(target, _snapDuration)
                .SetEase(_snapEase)
                .SetUpdate(_ignoreTimeScale);
        }

        #endregion

        #region Private

        private void CollectItems()
        {
            _collectBuffer.Clear();
            for (var i = 0; i < _content.childCount; i++)
            {
                if (_content.GetChild(i) is RectTransform child && child.gameObject.activeSelf)
                    _collectBuffer.Add(child);
            }

            var isChanged = _collectBuffer.Count != _items.Count;
            for (var i = 0; !isChanged && i < _items.Count; i++)
                isChanged = _collectBuffer[i] != _items[i];

            if (!isChanged)
                return;

            _items.Clear();
            _items.AddRange(_collectBuffer);

            _canvasGroups.Clear();
            foreach (var item in _items)
                _canvasGroups.Add(item.GetComponent<CanvasGroup>());

            _isPaddingDirty = true;
        }

        private void UpdatePadding()
        {
            if (!_isPaddingDirty || _scrollRect == null)
                return;

            _isPaddingDirty = false;

            if (!_autoPadding || _layoutGroup == null || _items.Count == 0)
                return;

            var viewSize = GetAxis(_viewport.rect.size);
            var start = Mathf.RoundToInt((viewSize - GetAxis(_items[0].rect.size)) * 0.5f);
            var end = Mathf.RoundToInt((viewSize - GetAxis(_items[^1].rect.size)) * 0.5f);

            var padding = _layoutGroup.padding;
            if (IsHorizontal)
            {
                padding.left = start;
                padding.right = end;
            }
            else
            {
                padding.top = start;
                padding.bottom = end;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
        }

        private void UpdateFocus()
        {
            if (_items.Count == 0)
                return;

            var center = GetAxis(_viewport.rect.center);
            var spacing = GetItemSpacing();
            var bestDistance = float.MaxValue;
            var bestIndex = -1;

            for (var i = 0; i < _items.Count; i++)
            {
                var distance = Mathf.Abs(GetAxis(GetItemCenter(_items[i])) - center);
                var normalized = distance / spacing;

                _items[i].localScale = Vector3.one * _scaleByDistance.Evaluate(normalized);

                if (_fadeByDistance && _canvasGroups[i] != null)
                    _canvasGroups[i].alpha = _alphaByDistance.Evaluate(normalized);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestIndex = i;
                }
            }

            SetCentered(bestIndex);
        }

        private int FindNearestIndex(float offset)
        {
            var center = GetAxis(_viewport.rect.center);
            var bestDistance = float.MaxValue;
            var bestIndex = -1;

            for (var i = 0; i < _items.Count; i++)
            {
                var distance = Mathf.Abs(GetAxis(GetItemCenter(_items[i])) + offset - center);
                if (distance >= bestDistance)
                    continue;

                bestDistance = distance;
                bestIndex = i;
            }

            return bestIndex;
        }

        private void SetCentered(int index)
        {
            if (_centeredIndex == index)
                return;

            _centeredIndex = index;
            OnCenteredChanged?.Invoke(index);
            _onCenteredChanged?.Invoke(index);
        }

        private Vector2 GetItemCenter(RectTransform item)
        {
            var localCenter = (Vector2)item.localPosition + item.rect.center;
            return _viewport.InverseTransformPoint(_content.TransformPoint(localCenter));
        }

        private float GetItemSpacing()
        {
            var spacing = _items.Count > 1
                ? Mathf.Abs(GetAxis(GetItemCenter(_items[1])) - GetAxis(GetItemCenter(_items[0])))
                : GetAxis(_items[0].rect.size);

            return Mathf.Max(spacing, 1f);
        }

        private float GetAxis(Vector2 value) => IsHorizontal ? value.x : value.y;

        #endregion

        #region Listeners

        public void OnBeginDrag(PointerEventData eventData)
        {
            _snapTween?.Kill();
            _snapTween = null;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var projection = GetAxis(_scrollRect.velocity) * _inertiaProjection;
            ScrollTo(FindNearestIndex(projection));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_scrollOnClick || eventData.dragging)
                return;

            var target = eventData.pointerPressRaycast.gameObject;
            if (target == null)
                return;

            for (var i = 0; i < _items.Count; i++)
            {
                if (!target.transform.IsChildOf(_items[i]))
                    continue;

                ScrollTo(i);
                return;
            }
        }

        #endregion
    }
}
#endif
