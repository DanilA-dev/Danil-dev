﻿using UnityEngine;

namespace D_Dev.UtilScripts.Entities.EntitiesInfo
{
    [CreateAssetMenu(menuName = "Project/EntityInfo")]
    public class EntityInfo : ScriptableObject
    {
        [field: SerializeField] public GameObject EntityPrefab { get; private set; }
    }
}