using UnityEngine;

namespace D_Dev.ValueSystem.RandomMethods
{
    [System.Serializable]
    public class DoubleRandomValueMethod : BaseRandomValueMethod<double>
    {
        #region Fields

        [SerializeField] private double _min;
        [SerializeField] private double _max;

        #endregion

        #region Properties

        public double Min
        {
            get => _min;
            set => _min = value;
        }

        public double Max
        {
            get => _max;
            set => _max = value;
        }

        #endregion

        #region Overrides

        public override double GetRandomValue()
        {
            return _min + (Random.value * (_max - _min));
        }

        #endregion
    }
}
