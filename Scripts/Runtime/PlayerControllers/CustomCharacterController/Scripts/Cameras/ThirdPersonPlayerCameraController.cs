using CustomCharacterController.Core;
using CustomCharacterController.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CustomCharacterController.Cameras
{
    public class ThirdPersonPlayerCameraController : MonoBehaviour, IPlayerCameraController
    {
        #region Fields

        [Title("Camera Settings")]
        [SerializeField] private Transform _cameraRoot;
        [SerializeField] private Transform _characterRoot;
        [SerializeField] private bool _rotateCharacterRootTransform = true;
        [SerializeField] private float _topAngle = 80f;
        [SerializeField] private float _botAngle = -80f;
        [SerializeField] private float _rotationSmoothTime;
        [SerializeField] private bool _isLocked;
        
        [FoldoutGroup("Events")]
        public UnityEvent<float> OnRotationValueChanged;
        [FoldoutGroup("Events")]
        public UnityEvent<Vector3> OnInputRotationChanged;
        
        private const float CAMERA_ROTATION_THRESHOLD = 0.01f;
        
        private IPlayerInputProvider _playerInputProvider;
        private IPlayerMovementCore _playerMovementCore;
        
        private Vector2 _currentLookInput;
        private Vector2 _movementInput;
        
        private float _cameraTargetPitch;
        private float _rotationVelocity;
        private float _targetRotation;
        private float _yaw;
        private float _pitch;

        private Camera _camera;
        private MovementPriorityChanger _thirdPersonCameraPriorityChanger;

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
            if (_playerInputProvider != null)
            {
                _playerInputProvider.Look -= OnLook;
                _playerInputProvider.Move -= OnMove;
            }
        }

        private void Update()
        {
            UpdateCharacterRotation();
        }

        private void LateUpdate()
        {
            UpdateCameraRotation();
        }

        #endregion

        #region Public


        public void Init(IPlayerControllerContext playerControllerContext)
        {
            _camera = Camera.main;
            if (playerControllerContext != null && playerControllerContext.PlayerInputProvider != null)
            {
                _playerInputProvider = playerControllerContext.PlayerInputProvider;
                _playerMovementCore = playerControllerContext.PlayerMovementCore;
                _playerInputProvider.Look += OnLook;
                _playerInputProvider.Move += OnMove;
                IsInitialized = true;

                _thirdPersonCameraPriorityChanger = new MovementPriorityChanger {Priority = 10, NewMovementDirection = Vector3.forward};
                _playerMovementCore?.SetInputMovementModifier(_thirdPersonCameraPriorityChanger);
            }
        }

      

        public void LockCamera() => _isLocked = true;

        public void UnlockCamera() => _isLocked = false;

        #endregion

        #region Listeners

        private void OnLook(Vector2 delta) => _currentLookInput = delta;
        private void OnMove(Vector2 moveInput) => _movementInput = moveInput;

        #endregion

        #region Private

        private void UpdateCameraRotation()
        {
            if (_currentLookInput.sqrMagnitude >= CAMERA_ROTATION_THRESHOLD && !_isLocked)
            {
                float deltaTimeMultiplier = 1.0f;

                _yaw += _currentLookInput.x * deltaTimeMultiplier;
                _pitch += _currentLookInput.y * deltaTimeMultiplier;
            }

            _yaw = Mathf.Clamp(_yaw, float.MinValue, float.MaxValue);
            _pitch = Mathf.Clamp(_pitch, _botAngle, _topAngle);

            _cameraRoot.rotation = Quaternion.Euler(_pitch,_yaw, 0.0f);
        }
        
        private void UpdateCharacterRotation()
        {
            if(_movementInput == Vector2.zero)
                return;
            
            var normalizedMovementInput = new Vector3(_movementInput.x, 0, _movementInput.y).normalized;
            
            _targetRotation = Mathf.Atan2(normalizedMovementInput.x, normalizedMovementInput.z) * Mathf.Rad2Deg +
                              _camera.transform.eulerAngles.y;
            var rotation = Mathf.SmoothDampAngle(_characterRoot.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                _rotationSmoothTime);
            
            if(_rotateCharacterRootTransform)
                _characterRoot.transform.rotation = Quaternion.Euler(0, rotation, 0);
            
            var targetDir = Quaternion.Euler(0, _targetRotation, 0) * Vector3.forward;
            
            OnInputRotationChanged?.Invoke(targetDir);
            OnRotationValueChanged?.Invoke(rotation);
            
            _thirdPersonCameraPriorityChanger.NewMovementDirection = targetDir;
        }
        #endregion
    }
}