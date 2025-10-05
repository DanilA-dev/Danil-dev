using D_Dev.ScriptableVaiables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_dev.TargetInfo
{
    #region Enums

    public enum TargetInfoType
    {
        Transform,
        Vector3,
        GameObjectVariable
    }

    #endregion
    
    [System.Serializable]
    public class TargetInfo
    {
        #region Fields

        [SerializeField] private TargetInfoType _targetInfoType;
        [ShowIf(nameof(_targetInfoType), TargetInfoType.Transform)]
        [SerializeField] private Transform _targetTransform;
        [ShowIf(nameof(_targetInfoType), TargetInfoType.Vector3)]
        [SerializeField] private Vector3 _targetVector3;
        [ShowIf(nameof(_targetInfoType), TargetInfoType.GameObjectVariable)]
        [SerializeField] private GameObjectScriptableVariable _targetGameObject;

        #endregion

        #region Public

        public Vector3 GetTargetPosition()
        {
            switch (_targetInfoType)
            {
                case TargetInfoType.Transform:
                    return _targetTransform.position;
                case TargetInfoType.Vector3:
                    return _targetVector3;
                case TargetInfoType.GameObjectVariable:
                    if (_targetGameObject == null || _targetGameObject.Value == null)
                        return Vector3.zero;
                    return _targetGameObject.Value.transform.position;
                default:
                    return Vector3.zero;
            }
        }
        
        #endregion
    }
}