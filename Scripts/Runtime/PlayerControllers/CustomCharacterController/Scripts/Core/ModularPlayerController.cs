using System.Linq;
using CustomCharacterController.Interfaces;
using UnityEngine;

namespace CustomCharacterController.Core
{
    public class ModularPlayerController : MonoBehaviour, IPlayerControllerContext
    {
        #region Fields

        private IPlayerCameraController _playerCameraController;
        private IPlayerInputProvider _playerInputProvider;
        private IPlayerMovementCore _playerMovementCore;
        private IPlayerCharacterAbility[] _abilities;
        private IPlayerContextDepender[] _playerContextModules;

        #endregion

        #region Properties

        public IPlayerCameraController PlayerCameraController => _playerCameraController;
        public IPlayerInputProvider PlayerInputProvider => _playerInputProvider;
        public IPlayerMovementCore PlayerMovementCore => _playerMovementCore;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            InitCoreComponents();
        }

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            UpdatePlayerAbilities();
        }

        private void FixedUpdate()
        {
            FixedUpdatePlayerAbilities();
        }

        private void LateUpdate()
        {
            LateUpdatePlayerAbilities();
        }

        #endregion

        #region Private

        private void InitCoreComponents()
        {
            _abilities = GetComponentsInChildren<IPlayerCharacterAbility>();
            _playerContextModules = GetComponents<IPlayerContextDepender>();
            
            if(!TryGetComponent(out _playerMovementCore))
                Debug.Log($"Can't find any movement core on {gameObject.name}");
            if(!TryGetComponent(out _playerCameraController))
                Debug.Log($"Can't find any camera controller on {gameObject.name}");
            if(!TryGetComponent(out _playerInputProvider))
                Debug.Log($"Can't find any input provider on {gameObject.name}");
        }
        
        private void Init()
        {
            if(_playerContextModules == null || _playerContextModules.Length == 0)
                return;

            foreach (var playerContextModule in _playerContextModules)
                playerContextModule.Init(this);
        }
        
        private void UpdatePlayerAbilities()
        {
            if (_abilities.Length > 0)
            {
                foreach (var playerCharacterAbility in _abilities)
                    playerCharacterAbility.TickUpdate();
            }
                
        }

        private void FixedUpdatePlayerAbilities()
        {
            if(_abilities.Length > 0)
                foreach (var playerCharacterAbility in _abilities)
                    playerCharacterAbility.TickFixedUpdate();
        }
        
        private void LateUpdatePlayerAbilities()
        {
            if(_abilities.Length > 0)
                foreach (var playerCharacterAbility in _abilities)
                    playerCharacterAbility.TickLateUpdate();
        }
        
        #endregion
    }
}