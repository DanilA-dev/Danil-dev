using System;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class PolymorphicValue<T>
    {
        #region Properties

        public abstract T Value { get; set; }

        #endregion

        #region Cloning

        public abstract PolymorphicValue<T> Clone();

        #endregion

        #region Overrides

        public virtual void Dispose() { }

        #endregion
    }
}
