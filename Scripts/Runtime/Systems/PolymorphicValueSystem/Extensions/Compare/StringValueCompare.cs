using UnityEngine;

namespace D_Dev.PolymorphicValueSystem.Compare
{
    public class StringValueCompare : BasePolymorphicValueCompare<string>
    {
        #region Public

        public override void CheckValue(string value)
        {
            string valueTo = _compareValueTo.Value;

            if (value == valueTo)
                OnValueCompareTrue?.Invoke();
            else
                OnValueCompareFalse?.Invoke();
        }

        #endregion
    }
}
