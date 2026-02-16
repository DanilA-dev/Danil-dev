using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.PolymorphicValueSystem.Compare
{
    public abstract class BasePolymorphicValueCompare<T> : MonoBehaviour
    {
        #region Fields

        [SerializeField,PropertyOrder(-1)] protected bool _checkOnStart;
        [SerializeReference] protected PolymorphicValue<T> _compareValue;
        [SerializeReference] protected PolymorphicValue<T> _compareValueTo;

        [FoldoutGroup("Events"), PropertyOrder(100)]
        [SerializeField] protected UnityEvent OnValuesEqual;

        #endregion

        #region Public

        public abstract void CheckValue(T value);
        public abstract void CheckValues();

        #endregion

        #region Monobehaviour

        private void Start()
        {
            if(_checkOnStart)
                CheckValues();
        }

        #endregion
    }
}