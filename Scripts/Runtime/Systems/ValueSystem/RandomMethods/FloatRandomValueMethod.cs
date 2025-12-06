using UnityEngine;

namespace D_Dev.ValueSystem.RandomMethods
{
    [System.Serializable]
    public class FloatRandomValueMethod : BaseRandomValueMethod<float>
    {
        #region Fields

        [SerializeField] private float _min;
        [SerializeField] private float _max;

        #endregion

        #region Properties

        public float Min
        {
            get => _min;
            set => _min = value;
        }

        public float Max
        {
            get => _max;
            set => _max = value;
        }

        #endregion

        #region Overrides

        public override float GetRandomValue()
        {
            return Random.Range(_min, _max);
        }

        #endregion
    }
}
