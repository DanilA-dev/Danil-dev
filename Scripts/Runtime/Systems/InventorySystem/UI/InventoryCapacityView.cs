using D_Dev.InventorySystem.Extensions;
using D_Dev.InventorySystem.UI.Filters;
using D_Dev.PolymorphicValueSystem;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace D_Dev.InventorySystem.UI
{
    public class InventoryCapacityView : MonoBehaviour
    {
        #region Enums

        public enum CountMode
        {
            OccupiedCells = 0,
            TotalAmount = 1
        }

        #endregion

        #region Fields

        [Title("References")]
        [SerializeReference] private PolymorphicValue<Inventory> _inventory = new InventoryScriptableVariableValue();
        [SerializeField, Required] private TMP_Text _text;
        [Title("Settings")]
        [SerializeField] private string _format = "{0}/{1}";
        [SerializeField] private CountMode _countMode = CountMode.OccupiedCells;
        [Title("Filter")]
        [SerializeReference] private InventorySlotFilter _filter;

        private Inventory _subscribedInventory;

        #endregion

        #region Properties

        public int Count { get; private set; }
        public int Capacity { get; private set; }

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            if (_inventory != null)
                _inventory.OnValueChanged += OnInventoryChanged;

            Subscribe(_inventory?.Value);
            Refresh();
        }

        private void Start() => Refresh();

        private void OnDisable()
        {
            if (_inventory != null)
                _inventory.OnValueChanged -= OnInventoryChanged;

            Subscribe(null);
        }

        #endregion

        #region Public

        public void Refresh()
        {
            Count = 0;
            Capacity = 0;

            if (_subscribedInventory != null)
            {
                Capacity = _subscribedInventory.Cells.Count;

                foreach (var cell in _subscribedInventory.Cells.Values)
                {
                    if (cell.IsEmpty)
                        continue;

                    if (_filter != null && !_filter.IsValid(cell.Data.Info))
                        continue;

                    Count += _countMode == CountMode.TotalAmount ? cell.Data.CurrentAmount : 1;
                }
            }

            if (_text != null)
                _text.text = string.Format(_format, Count, Capacity);
        }

        #endregion

        #region Private

        private void Subscribe(Inventory inventory)
        {
            if (_subscribedInventory != null)
            {
                _subscribedInventory.OnItemAdded -= OnItemsChanged;
                _subscribedInventory.OnItemRemoved -= OnItemsChanged;
                _subscribedInventory.OnCellsChanged -= Refresh;
            }

            _subscribedInventory = inventory;

            if (_subscribedInventory != null)
            {
                _subscribedInventory.OnItemAdded += OnItemsChanged;
                _subscribedInventory.OnItemRemoved += OnItemsChanged;
                _subscribedInventory.OnCellsChanged += Refresh;
            }
        }

        #endregion

        #region Listeners

        private void OnInventoryChanged(Inventory inventory)
        {
            Subscribe(inventory);
            Refresh();
        }

        private void OnItemsChanged(InventoryItemEntityInfo info, int amount) => Refresh();

        #endregion
    }
}
