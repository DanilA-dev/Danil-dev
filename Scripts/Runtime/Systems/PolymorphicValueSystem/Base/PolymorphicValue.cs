using System;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace D_Dev.PolymorphicValueSystem
{
    [System.Serializable]
    public abstract class PolymorphicValue<T>
    {
        #region Fields

        public Action<T> OnValueChanged;

        #endregion
        
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
