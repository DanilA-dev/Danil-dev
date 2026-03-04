namespace D_Dev.ScriptableVaiables.Setters
{
    public class DoubleScriptableVariableSetter : BaseScriptableVariableSetter<double, DoubleScriptableVariable>
    {
        #region Public
        public void AddValue(double value)
        {
            _value += value;
            _variable.Value = _value;
        }

        public void Subtract(double value)
        {
            _value -= value;
            _variable.Value = _value;
        }
        #endregion
    }
}
