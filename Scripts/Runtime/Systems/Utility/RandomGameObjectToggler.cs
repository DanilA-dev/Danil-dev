using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.Utility
{
    public class RandomGameObjectToggler : MonoBehaviour
    {
        #region Fields

        [SerializeField] private bool _toggleOnAwake = true;
        [SerializeField] private GameObject[] _objects;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            if (_toggleOnAwake)
                Toggle();
        }

        #endregion

        #region Public

        [Button]
        public void Toggle()
        {
            if (_objects == null || _objects.Length == 0)
                return;

            var index = Random.Range(0, _objects.Length);
            for (var i = 0; i < _objects.Length; i++)
            {
                if (_objects[i] != null)
                    _objects[i].SetActive(i == index);
            }
        }

        #endregion
    }
}
