using System;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class PolymorphicValue<T>
    {
        #region Properties

        public abstract T Value { get; set; }

        public event Action<T, T> OnValueChanged;

        #endregion

        #region Cloning

        public abstract PolymorphicValue<T> Clone();

        #endregion

        #region Overrides

        public virtual void Dispose() { }

        #endregion

        protected void RaiseOnValueChanged(T oldValue, T newValue)
        {
            OnValueChanged?.Invoke(oldValue, newValue);
        }
    }
}
