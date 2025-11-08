using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.Base
{
    [CreateAssetMenu(menuName = "D-Dev/Info/EntityInfo")]
    public class EntityInfo : ScriptableObject
    {
        #region Fields

        [InlineButton(nameof(CreateID)), PropertyOrder(-1)]
        [SerializeField] private string _id;
        [PropertySpace(10)]
        [SerializeField] private GameObject _entityPrefab;

        #endregion

        #region Properties

        public string ID => _id;
        public GameObject EntityPrefab => _entityPrefab;

        #endregion

        #region Private

        private void CreateID() => _id = System.Guid.NewGuid().ToString();

        #endregion
    }
}
