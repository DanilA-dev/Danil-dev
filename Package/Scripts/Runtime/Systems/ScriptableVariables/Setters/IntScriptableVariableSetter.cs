namespace D_Dev.ScriptableVaiables.Setters
{
    public class IntScriptableVariableSetter : BaseScriptableVariableSetter<int, IntScriptableVariable>
    {
        #region Public
        public void AddValue(int value)
        {
            _value += value;
            _variable.Value = _value;
        }

        public void Subtract(int value)
        {
            _value -= value;
            _variable.Value = _value;
        }
        #endregion
    }
}
