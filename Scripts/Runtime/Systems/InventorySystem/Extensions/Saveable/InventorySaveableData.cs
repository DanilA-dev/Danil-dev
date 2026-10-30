using System;
using System.Collections.Generic;
using System.Linq;
using D_Dev.Entity;
using D_Dev.PolymorphicValueSystem;
using D_Dev.SaveSystem.SaveableData;
using UnityEngine;

namespace D_Dev.InventorySystem.Extensions
{
    [Serializable]
    public class InventorySaveEntry
    {
        public string Id;
        public int Amount;
    }

    [Serializable]
    public class InventorySaveableData : BaseSaveableData<List<InventorySaveEntry>>
    {
        #region Fields

        [SerializeReference] private PolymorphicValue<Inventory> _inventory = new InventoryScriptableVariableValue();
        [SerializeField] private EntityInfoDatabase _database;

        private List<InventorySaveEntry> _pending;
        private List<InventorySaveEntry> _snapshot;
        private Inventory _boundInventory;
        private bool _isWatching;
        private bool _isApplying;

        #endregion

        #region Properties

        public override bool CanSave => IsInventoryAlive || _snapshot != null || _pending != null;

        private bool IsInventoryAlive => _boundInventory != null;

        #endregion

        #region Subscription

        public override void Subscribe() => Watch();

        public override void Unsubscribe()
        {
            if (!_isWatching)
                return;

            _isWatching = false;

            if (_inventory != null)
                _inventory.OnValueChanged -= OnInventoryChanged;

            Bind(null);
        }

        #endregion

        #region Overrides

        protected override List<InventorySaveEntry> GetTypedSaveData()
        {
            if (IsInventoryAlive)
                _snapshot = Capture(_boundInventory);

            return _snapshot ?? _pending ?? new List<InventorySaveEntry>();
        }

        protected override void SetTypedSaveData(List<InventorySaveEntry> data)
        {
            _pending = data ?? new List<InventorySaveEntry>();
            Watch();
            Bind(_inventory?.Value);
        }

        protected override List<InventorySaveEntry> GetTypedDefaultValue() => new();

        #endregion

        #region Private

        private void Watch()
        {
            if (_isWatching || _inventory == null)
                return;

            _isWatching = true;
            _inventory.OnValueChanged += OnInventoryChanged;
            Bind(_inventory.Value);
        }

        private void Bind(Inventory inventory)
        {
            if (inventory == _boundInventory && inventory != null)
            {
                ApplyPending();
                return;
            }

            if (_boundInventory != null)
            {
                _boundInventory.OnItemAdded -= OnItemsChanged;
                _boundInventory.OnItemRemoved -= OnItemsChanged;
            }

            _boundInventory = inventory;

            if (_boundInventory == null)
                return;

            _boundInventory.OnItemAdded += OnItemsChanged;
            _boundInventory.OnItemRemoved += OnItemsChanged;
            ApplyPending();
        }

        private void ApplyPending()
        {
            if (_pending == null || !IsInventoryAlive)
                return;

            var data = _pending;
            _pending = null;
            _isApplying = true;

            foreach (var cell in _boundInventory.Cells.Values.ToList())
            {
                if (!cell.IsEmpty)
                    _boundInventory.RemoveItem(cell.Data.Info, cell.Data.CurrentAmount);
            }

            foreach (var entry in data)
            {
                if (entry == null || entry.Amount <= 0 || _database == null)
                    continue;

                if (_database.TryGet(entry.Id, out var entity) && entity is InventoryItemEntityInfo item)
                    _boundInventory.AddItem(item, entry.Amount);
            }

            _isApplying = false;
            _snapshot = Capture(_boundInventory);
        }

        private static List<InventorySaveEntry> Capture(Inventory inventory)
        {
            var entries = new List<InventorySaveEntry>();
            foreach (var pair in inventory.Cells.OrderBy(p => p.Key))
            {
                var cell = pair.Value;
                if (cell.IsEmpty || cell.Data.Info == null)
                    continue;

                entries.Add(new InventorySaveEntry { Id = cell.Data.Info.ID, Amount = cell.Data.CurrentAmount });
            }

            return entries;
        }

        #endregion

        #region Listeners

        private void OnInventoryChanged(Inventory inventory) => Bind(inventory);

        private void OnItemsChanged(InventoryItemEntityInfo item, int amount)
        {
            if (_isApplying || !IsInventoryAlive)
                return;

            _snapshot = Capture(_boundInventory);
            NotifyChanged();
        }

        #endregion
    }
}
