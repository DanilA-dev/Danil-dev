using System;
using D_Dev.PolymorphicValueSystem;
using D_Dev.SaveSystem.SaveableData;
using UnityEngine;

namespace D_Dev.Entity.Extensions
{
    [Serializable]
    public class EntityInfoValueSaveableData : BaseSaveableData<string>
    {
        #region Fields

        [SerializeReference] private PolymorphicValue<EntityInfo> _value = new EntityInfoScriptableVariableValue();
        [SerializeReference] private PolymorphicValue<EntityInfo> _defaultValue = new EntityInfoConstantValue();
        [SerializeField] private EntityInfoDatabase _database;

        #endregion

        #region Subscription

        public override void Subscribe()
        {
            if (_value != null)
                _value.OnValueChanged += OnValueChanged;
        }

        public override void Unsubscribe()
        {
            if (_value != null)
                _value.OnValueChanged -= OnValueChanged;
        }

        private void OnValueChanged(EntityInfo value) => NotifyChanged();

        #endregion

        #region Overrides

        protected override string GetTypedSaveData() => _value?.Value != null ? _value.Value.ID : string.Empty;

        protected override void SetTypedSaveData(string data)
        {
            if (_value != null && _database != null && _database.TryGet(data, out var entity))
                _value.Value = entity;
        }

        protected override string GetTypedDefaultValue() => _defaultValue?.Value != null ? _defaultValue.Value.ID : string.Empty;

        #endregion
    }
}
