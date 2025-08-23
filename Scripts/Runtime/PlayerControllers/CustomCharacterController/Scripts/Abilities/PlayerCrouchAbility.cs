using System.Collections;
using CustomCharacterController.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CustomCharacterController.Abilities
{
    public class PlayerCrouchAbility : BasePlayerCharacterAbility
    {
        #region Fields

        [SerializeField, Required] private CharacterController _characterController;
        [SerializeField] private float _crouchHorizontalSpeed;
        [SerializeField] private float _crouchHeight;
        [SerializeField] private float _crouchTime;
        
        [FoldoutGroup("Events")]
        public UnityEvent<bool> OnCrouch;
        [FoldoutGroup("Events")] 
        public UnityEvent<float> OnCrouchHeightChange;
        
        private float _defaultHeight;
        private float _defaultHorizontalSpeed;
        private bool _isCrouchingRoutine;
        
        #endregion

        #region Monobehaviour

        private void OnDestroy()
        {
            _playerInputProvider.CrouchPressed -= OnCrouchPressed;
        }

        #endregion
        
        #region Overrides

        protected override void OnInitialize()
        {
            _defaultHeight = _characterController.height;
            _defaultHorizontalSpeed = _playerMovementCore.MaxHorizontalSpeed;
            _playerInputProvider.CrouchPressed += OnCrouchPressed;
        }

        #endregion
        
        #region Listeners

        private void OnCrouchPressed(bool isCrouch)
        {
            if(IsBlockedByExecutingAbilities())
                return;

            if (!IsActive)
            {
                _playerMovementCore.MaxHorizontalSpeed = _defaultHorizontalSpeed;
                _characterController.height = _defaultHeight;
                return;
            }
            
            var endHeight = isCrouch? _crouchHeight : _defaultHeight;
            _playerMovementCore.MaxHorizontalSpeed = isCrouch 
                ? _crouchHorizontalSpeed 
                : _defaultHorizontalSpeed;
            
            _isExecuting = isCrouch;
            OnCrouch?.Invoke(_isExecuting);
            
            if(!_isCrouchingRoutine)
                StartCoroutine(CrouchRoutine(_crouchTime, endHeight));
        }

        #endregion
        
        #region IEnumerators

        private IEnumerator CrouchRoutine(float duration, float endHeight)
        {
            if (_characterController == null)
                yield break;
                
            float startHeight = _characterController.height;
            float elapsedTime = 0f;
            _isCrouchingRoutine = true;
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                _characterController.height = Mathf.Lerp(startHeight, endHeight, t);
                OnCrouchHeightChange?.Invoke(_characterController.height);
                yield return null;
            }
            _characterController.height = endHeight;
            _isCrouchingRoutine = false;
            
            if(!_isExecuting && Mathf.Approximately(_characterController.height, _crouchHeight))
                yield return StartCoroutine(CrouchRoutine(_crouchTime, _defaultHeight));
        }
        #endregion
    }
}