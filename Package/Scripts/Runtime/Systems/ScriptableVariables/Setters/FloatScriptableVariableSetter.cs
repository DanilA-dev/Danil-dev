namespace D_Dev.ScriptableVaiables.Setters
{
    public class FloatScriptableVariableSetter : BaseScriptableVariableSetter<float, FloatScriptableVariable>
    {
        #region Public
        public void AddValue(float value)
        {
            _value += value;
            _variable.Value = _value;
        }

        public void Subtract(float value)
        {
            _value -= value;
            _variable.Value = _value;
        }
        #endregion
    }
}
