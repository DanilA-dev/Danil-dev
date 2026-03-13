using System;
using D_Dev.PolymorphicValueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.SaveSystem.SaveableData
{
    [Serializable]
    public abstract class BaseSaveableData
    {
        #region Fields

        [Title("Saved Key"), PropertyOrder(-1)]
        [SerializeReference] protected PolymorphicValue<string> _key = new StringConstantValue();
        [Title("Default Save/Load Settings"), PropertyOrder(100)]
        [SerializeField] private bool _saveOnExit = true;
        [PropertyOrder(100)]
        [SerializeField] private bool _loadOnStart = true;

        #endregion

        #region Properties
        
        public PolymorphicValue<string> Key
        {
            get => _key;
            set => _key = value;
        }

        public bool SaveOnExit
        {
            get => _saveOnExit;
            set => _saveOnExit = value;
        }

        public bool LoadOnStart
        {
            get => _loadOnStart;
            set => _loadOnStart = value;
        }

        #endregion

        #region Public

        public abstract object GetSaveData();
        public abstract void SetSaveData(object data);
        public abstract object GetDefaultValue();

        #endregion
    }
    
    [Serializable]
    public abstract class BaseSaveableData<TData> : BaseSaveableData
    {
        #region Fields

        [FoldoutGroup("Events"), PropertyOrder(100)]
        public UnityEvent<TData> OnSaveData;
        [FoldoutGroup("Events"), PropertyOrder(100)]
        public UnityEvent<TData> OnLoadData;

        #endregion
        
        #region Overrides

        public override object GetSaveData()
        {
            var saveData = GetTypedSaveData();
            OnLoadData?.Invoke(saveData);
            return saveData;
        }

        public override object GetDefaultValue() => GetTypedDefaultValue();

        public override void SetSaveData(object data)
        {
            if (data is TData typedData)
            {
                SetTypedSaveData(typedData);
                OnSaveData?.Invoke(typedData);
                return;
            }

            try
            {
                var converted = (TData)Convert.ChangeType(data, typeof(TData));
                SetTypedSaveData(converted);
                OnSaveData?.Invoke(converted);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveableData] Type mismatch for key '{Key.Value}': " +
                                 $"expected {typeof(TData).Name}, got {data?.GetType().Name}. " +
                                 $"Conversion failed: {e.Message}");
            }
        }

        #endregion
        
        #region Protected

        protected abstract TData GetTypedSaveData();
        protected abstract void SetTypedSaveData(TData data);
        protected abstract TData GetTypedDefaultValue();

        #endregion
    }
}