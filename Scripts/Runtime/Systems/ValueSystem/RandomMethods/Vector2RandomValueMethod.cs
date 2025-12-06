using UnityEngine;

namespace D_Dev.ValueSystem.RandomMethods
{
    [System.Serializable]
    public class Vector2RandomValueMethod : BaseRandomValueMethod<Vector2>
    {
        #region Fields

        [SerializeField] private Vector2 _min;
        [SerializeField] private Vector2 _max;

        #endregion

        #region Properties

        public Vector2 Min
        {
            get => _min;
            set => _min = value;
        }

        public Vector2 Max
        {
            get => _max;
            set => _max = value;
        }

        #endregion

        #region Overrides

        public override Vector2 GetRandomValue()
        {
            float x = UnityEngine.Random.Range(_min.x, _max.x);
            float y = UnityEngine.Random.Range(_min.y, _max.y);
            return new Vector2(x, y);
        }

        #endregion
    }
}
