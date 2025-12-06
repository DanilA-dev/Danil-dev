using Cinemachine;
using D_Dev.ValueSystem;
using UnityEngine;

namespace D_Dev.Utility
{
    public class CinemachineTargetAutoAssigner : MonoBehaviour
    {
        #region Fields

        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private GameObjectValue _target;
        [Space]
        [SerializeField] private bool _isFollow;
        [SerializeField] private bool _isLookAt;

        #endregion

        #region MonoBehaviour

        private void Awake() => _target.OnValueChanged += SetTarget;
        private void OnDestroy() => _target.OnValueChanged -= SetTarget;

        #endregion

        #region Listeners

        private void SetTarget(GameObject target)
        {
            _camera.Follow = _isFollow ? target.transform : null;
            _camera.LookAt = _isLookAt ? target.transform : null;
        }

        #endregion
    }
}
