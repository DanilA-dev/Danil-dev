using System.Collections.Generic;
using System.Linq;
using CustomCharacterController.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CustomCharacterController.Core
{
    public abstract class BasePlayerCharacterAbility : MonoBehaviour, IPlayerCharacterAbility
    {
        #region Fields

        [Title("Base")]
        [GUIColor(nameof(GetActiveColor))]
        [SerializeField] private bool _isActive;
        [SerializeField, ReadOnly] private bool _isExecuting;
        [SerializeField] protected List<BasePlayerCharacterAbility> _blockedByExecutingAbilities = new();
        [FoldoutGroup("Events")]
        public UnityEvent OnAbilityActivated;
        [FoldoutGroup("Events")]
        public UnityEvent OnAbilityDeactivated;
        [FoldoutGroup("Events")]
        public UnityEvent<bool> OnAbilityExecuting;

        protected IPlayerCameraController _playerCameraController;
        protected IPlayerInputProvider _playerInputProvider;
        protected IPlayerMovementCore _playerMovementCore;
        
        #endregion
        
        #region Properties
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value && IsBlockedByExecutingAbilities())
                    _isActive = false;
                else
                    _isActive = value;
                
                if(!_isActive)
                    _isExecuting = false;
                
                if(_isActive)
                    OnAbilityActivated?.Invoke();
                else
                    OnAbilityDeactivated?.Invoke();
            }
        }

        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                _isExecuting = value;
                OnAbilityExecuting?.Invoke(value);
            }
        }

        public bool IsInitialized {get; private set;}
        
        #endregion

        #region Public


        public void Init(IPlayerControllerContext playerControllerContext)
        {
            _playerCameraController = playerControllerContext.PlayerCameraController;
            _playerInputProvider = playerControllerContext.PlayerInputProvider;
            _playerMovementCore = playerControllerContext.PlayerMovementCore;
            
            OnInitialize();
            IsInitialized = true;
        }

        public void TickUpdate()
        {
            if(!IsActive)
                return;

            if (IsBlockedByExecutingAbilities())
                return;
            
            OnTickUpdate();
        }

        public void TickFixedUpdate()
        {
            if(!IsActive)
                return;
            
            if(IsBlockedByExecutingAbilities())
                return;
            
            OnTickFixedUpdate();
        }

        public void TickLateUpdate()
        {
            if(!IsActive)
                return;
            
            if(IsBlockedByExecutingAbilities())
                return;
            
            OnTickLateUpdate();
        }

        

        #endregion

        #region Protected

        protected bool IsBlockedByExecutingAbilities()
        {
            var blocked = _blockedByExecutingAbilities.Count > 0 
                          && _blockedByExecutingAbilities.Any(ability => ability.IsExecuting);
            return blocked;
        }
        

        #endregion
        
        #region Virutal

        protected virtual void OnInitialize() { }

        protected virtual void OnTickUpdate() { }

        protected virtual void OnTickFixedUpdate() { }

        protected virtual void OnTickLateUpdate() { }

        #endregion

        #region Debug

        private Color GetActiveColor() => IsActive ? Color.green : Color.red;

        #endregion

        
    }
}