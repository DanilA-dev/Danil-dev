using UnityEngine;

namespace D_Dev.ValueSystem.RandomMethods
{
    [System.Serializable]
    public class IntRandomValueMethod : BaseRandomValueMethod<int>
    {
        #region Fields

        [SerializeField] private int _min;
        [SerializeField] private int _max;

        #endregion

        #region Properties

        public int Min
        {
            get => _min;
            set => _min = value;
        }

        public int Max
        {
            get => _max;
            set => _max = value;
        }

        #endregion

        #region Overrides

        public override int GetRandomValue()
        {
            return Random.Range(_min, _max);
        }

        #endregion
    }
}