using CustomCharacterController.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomCharacterController.Cameras
{
    public class FirstPersonPlayerCameraController : MonoBehaviour, IPlayerCameraController
    {
        #region Fields

        [Title("Camera Settings")]
        [SerializeField] private Transform _cameraRoot;
        [SerializeField] private Transform _characterRoot;
        [SerializeField] private float _lookAngle = 80f;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private bool _isLocked;
        
        private const float CAMERA_ROTATION_THRESHOLD = 0.01f;
        
        private Vector2 _currentLookInput;
        private float _cameraTargetPitch;
        private float _rotationVelocity;
        private IPlayerInputProvider _playerInputProvider;

        #endregion

        #region Properties

        public Transform CameraRoot => _cameraRoot;
        public Vector3 CameraForward => _cameraRoot.forward;
        public Vector3 CameraRight => _cameraRoot.right;

        public bool IsInitialized { get; private set; }

        #endregion

        #region Monobehaviour

        private void OnDestroy()
        {
            if(_playerInputProvider != null)
                _playerInputProvider.Look -= OnLook;
        }

        private void LateUpdate()
        {
            UpdateCameraRotation();
        }
        
        #endregion
        
        #region Public

        public void Init(IPlayerControllerContext playerControllerContext)
        {
            if (playerControllerContext.PlayerInputProvider != null)
            {
                _playerInputProvider = playerControllerContext.PlayerInputProvider;
                _playerInputProvider.Look += OnLook;
                IsInitialized = true;
            }
        }

        public void LockCamera() => _isLocked = true;
        public void UnlockCamera() => _isLocked = false;

        #endregion

        #region Private

        private void UpdateCameraRotation()
        {
            if (_currentLookInput.sqrMagnitude < CAMERA_ROTATION_THRESHOLD || _isLocked)
                return;

            var camMovementDelta = 1;
            _cameraTargetPitch += _currentLookInput.y * _rotationSpeed * camMovementDelta;
            _cameraTargetPitch = Mathf.Clamp(_cameraTargetPitch, -_lookAngle, _lookAngle);
            
            _rotationVelocity = _currentLookInput.x * _rotationSpeed * camMovementDelta;
            
            _cameraRoot.localRotation = Quaternion.Euler(_cameraTargetPitch, 0, 0);
            _characterRoot.Rotate(Vector3.up * _rotationVelocity);
        }

        #endregion

        #region Listeners

        private void OnLook(Vector2 lookDelta)
        {
            _currentLookInput = lookDelta;
        }

        #endregion

       
    }
}