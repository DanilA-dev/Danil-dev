using UnityEngine;

namespace D_Dev.PolymorphicValueSystem.Compare
{
    public class BoolValueCompare : BasePolymorphicValueCompare<bool>
    {
        #region Public

        public override void CheckValue(bool value)
        {
            bool valueTo = _compareValueTo.Value;

            if (value == valueTo)
                OnValueCompareTrue?.Invoke();
            else
                OnValueCompareFalse?.Invoke();
        }

        #endregion
    }
}
