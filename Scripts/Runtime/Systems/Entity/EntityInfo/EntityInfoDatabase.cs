using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace D_Dev.Entity
{
    [CreateAssetMenu(menuName = "D-Dev/Entity/Entity Info Database")]
    public class EntityInfoDatabase : ScriptableObject
    {
        #region Fields

        [Title("Collect")]
        [SerializeField, FolderPath] private List<string> _searchFolders = new();
        [Title("Entities")]
        [SerializeField] private List<EntityInfo> _entities = new();

        private Dictionary<string, EntityInfo> _lookup;

        #endregion

        #region Properties

        public IReadOnlyList<EntityInfo> Entities => _entities;

        #endregion

        #region ScriptableObject

        private void OnValidate() => _lookup = null;

        #endregion

        #region Public

        public bool TryGet(string id, out EntityInfo entity)
        {
            entity = null;
            if (string.IsNullOrEmpty(id))
                return false;

            if (_lookup == null)
                BuildLookup();

            return _lookup.TryGetValue(id, out entity) && entity != null;
        }

        #endregion

        #region Private

        private void BuildLookup()
        {
            _lookup = new Dictionary<string, EntityInfo>();
            foreach (var entity in _entities)
            {
                if (entity != null && !string.IsNullOrEmpty(entity.ID))
                    _lookup[entity.ID] = entity;
            }
        }

#if UNITY_EDITOR
        [Button]
        private void CollectFromFolders()
        {
            var folders = _searchFolders.FindAll(folder => !string.IsNullOrEmpty(folder) && AssetDatabase.IsValidFolder(folder));
            if (folders.Count == 0)
            {
                Debug.LogWarning($"{nameof(EntityInfoDatabase)}: no valid search folders", this);
                return;
            }

            var collected = new List<EntityInfo>();
            foreach (var guid in AssetDatabase.FindAssets($"t:{nameof(EntityInfo)}", folders.ToArray()))
            {
                var entity = AssetDatabase.LoadAssetAtPath<EntityInfo>(AssetDatabase.GUIDToAssetPath(guid));
                if (entity != null && !collected.Contains(entity))
                    collected.Add(entity);
            }

            Undo.RecordObject(this, "Collect Entities");
            _entities = collected;
            _lookup = null;
            EditorUtility.SetDirty(this);
            Debug.Log($"{nameof(EntityInfoDatabase)}: collected {collected.Count} entities", this);
        }
#endif

        #endregion
    }
}
