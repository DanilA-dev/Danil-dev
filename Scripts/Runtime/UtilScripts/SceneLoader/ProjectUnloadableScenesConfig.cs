using System.Collections.Generic;
using UnityEngine;

namespace D_Dev.SceneLoader
{
    [CreateAssetMenu(menuName = "D-Dev/ProjectUnloadableScenesConfig")]
    public class ProjectUnloadableScenesConfig : ScriptableObject
    {
        #region Fields

        [SerializeField] private List<SceneInfo> _scenes = new();

        #endregion

        #region Properties

        public List<SceneInfo> Scenes => _scenes;

        #endregion
    }
}