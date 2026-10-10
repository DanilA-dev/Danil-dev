using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.InventorySystem.UI
{
    public class InventoryView : MonoBehaviour
    {
        #region Fields

        [Title("Slots")]
        [SerializeField, Required] private InventorySlotView _slotPrefab;
        [SerializeField, Required] private RectTransform _content;
        [Title("Selection")]
        [SerializeField] private bool _selectable = true;

        [PropertyOrder(100)]
        [FoldoutGroup("Events")]
        public UnityEvent<InventorySlotView> OnSlotSelectedEvent;

        private readonly List<InventorySlotView> _slots = new();
        private Inventory _inventory;
        private InventorySlotView _selectedSlot;

        public event Action<InventorySlotView> OnSlotSelected;

        #endregion

        #region Properties

        public Inventory Inventory => _inventory;
        public IReadOnlyList<InventorySlotView> Slots => _slots;
        public InventorySlotView SelectedSlot => _selectedSlot;

        #endregion

        #region Monobehaviour

        private void OnDestroy()
        {
            foreach (var slot in _slots)
            {
                if (slot != null)
                    slot.OnClicked -= OnSlotClicked;
            }
        }

        #endregion

        #region Public

        public void Bind(Inventory inventory)
        {
            Unbind();

            _inventory = inventory;
            if (_inventory == null)
                return;

            var cellsCount = _inventory.Cells.Count;
            CreateMissingSlots(cellsCount);

            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                var isUsed = i < cellsCount;
                slot.gameObject.SetActive(isUsed);

                if (isUsed)
                    slot.Bind(_inventory.Cells[i]);
            }
        }

        public void Unbind()
        {
            Deselect();

            foreach (var slot in _slots)
                slot.Unbind();

            _inventory = null;
        }

        public void Select(InventorySlotView slot)
        {
            if (!_selectable || slot == null || slot == _selectedSlot)
                return;

            Deselect();

            _selectedSlot = slot;
            slot.Select();
            OnSlotSelected?.Invoke(slot);
            OnSlotSelectedEvent?.Invoke(slot);
        }

        public void Deselect()
        {
            if (_selectedSlot == null)
                return;

            _selectedSlot.Deselect();
            _selectedSlot = null;
        }

        #endregion

        #region Private

        private void CreateMissingSlots(int count)
        {
            for (int i = _slots.Count; i < count; i++)
            {
                var slot = Instantiate(_slotPrefab, _content);
                slot.OnClicked += OnSlotClicked;
                _slots.Add(slot);
            }
        }

        #endregion

        #region Listeners

        private void OnSlotClicked(InventorySlotView slot) => Select(slot);

        #endregion
    }
}
