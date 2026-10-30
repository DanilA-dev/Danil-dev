using System;
using System.Collections.Generic;
using D_Dev.SaveSystem.SaveableData;
using UnityEngine;

namespace D_Dev.ShopSystem.Extensions
{
    [Serializable]
    public class ShopItemsContainerSaveableData : BaseSaveableData<Dictionary<string, int>>
    {
        #region Fields

        [SerializeField] private ShopItemsContainer _container;

        #endregion

        #region Subscription

        public override void Subscribe()
        {
            if (_container == null)
                return;

            foreach (var item in _container.Items)
            {
                if (item != null)
                    item.OnChanged += OnItemChanged;
            }
        }

        public override void Unsubscribe()
        {
            if (_container == null)
                return;

            foreach (var item in _container.Items)
            {
                if (item != null)
                    item.OnChanged -= OnItemChanged;
            }
        }

        private void OnItemChanged() => NotifyChanged();

        #endregion

        #region Overrides

        protected override Dictionary<string, int> GetTypedSaveData()
        {
            var data = new Dictionary<string, int>();
            if (_container == null)
                return data;

            foreach (var item in _container.Items)
            {
                if (item != null && item.TryGetSaveState(out var state))
                    data[item.name] = state;
            }

            return data;
        }

        protected override void SetTypedSaveData(Dictionary<string, int> data)
        {
            if (_container == null || data == null)
                return;

            foreach (var item in _container.Items)
            {
                if (item != null && data.TryGetValue(item.name, out var state))
                    item.LoadSaveState(state);
            }
        }

        protected override Dictionary<string, int> GetTypedDefaultValue() => new();

        #endregion
    }
}
