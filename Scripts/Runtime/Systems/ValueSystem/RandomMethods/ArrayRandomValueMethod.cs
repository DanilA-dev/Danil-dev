using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace D_Dev.ValueSystem.RandomMethods
{
    [Serializable]
    public class ArrayRandomValueMethod<T> : BaseRandomValueMethod<T>
    {
        #region Fields

        [SerializeField] protected T[] _values;

        #endregion
        
        #region Properties
        
        protected T[] Values
        {
            get => _values;
            set => _values = value;
        }
        
        #endregion

        #region Overrides

        public override T GetRandomValue()
        {
            return Values[Random.Range(0, Values.Length)];
        }

        #endregion
    }
}