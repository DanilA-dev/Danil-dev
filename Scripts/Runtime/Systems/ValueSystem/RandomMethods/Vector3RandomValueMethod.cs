using UnityEngine;

namespace D_Dev.ValueSystem.RandomMethods
{
    [System.Serializable]
    public class Vector3RandomValueMethod : BaseRandomValueMethod<Vector3>
    {
        #region Fields

        [SerializeField] private Vector3 _min;
        [SerializeField] private Vector3 _max;

        #endregion

        #region Properties

        public Vector3 Min
        {
            get => _min;
            set => _min = value;
        }

        public Vector3 Max
        {
            get => _max;
            set => _max = value;
        }

        #endregion

        #region Overrides

        public override Vector3 GetRandomValue()
        {
            float x = UnityEngine.Random.Range(_min.x, _max.x);
            float y = UnityEngine.Random.Range(_min.y, _max.y);
            float z = UnityEngine.Random.Range(_min.z, _max.z);
            return new Vector3(x, y, z);
        }

        #endregion
    }
}
