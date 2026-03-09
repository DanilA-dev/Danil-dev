using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace D_Dev.PositionRotationConfig
{
    [Serializable]
    public class BasePositionSettings
    {
        #region Fields

        [FoldoutGroup("Random")]
        [SerializeField] private bool _useRandomSphere;
        [FoldoutGroup("Random"), ShowIf(nameof(_useRandomSphere))]
        [SerializeField] private float _radius;

        #endregion

        #region Properties

        public bool UseRandomSphere
        {
            get => _useRandomSphere;
            set => _useRandomSphere = value;
        }

        public float RandomRadius
        {
            get => _radius;
            set => _radius = value;
        }

        #endregion
        
        #region Public

        public Vector3 GetPosition()
        {
            Vector3 randomOffset = Random.insideUnitSphere * _radius;
            randomOffset.y = 0;

            if (_useRandomSphere)
                return OnGetPosition() + randomOffset;
                
            return OnGetPosition();
        }

        #endregion

        #region Virtual

        public virtual Vector3 OnGetPosition() => Vector3.zero;

        #endregion
    }
}