using System.Collections.Generic;
using System.Linq;
using CustomCharacterController.Interfaces;
using D_Dev.ColliderEvents;
using D_Dev.Raycaster;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomCharacterController.Core
{
    public class MovementPriorityChanger
    {
        public int Priority;
        public Vector3 NewMovementDirection;
    }
    
    public class CharacterControllerPlayerMovementCore : MonoBehaviour, IPlayerMovementCore
    {
        #region Enums

        private enum GroundCheckType
        {
            Raycast,
            Trigger
        }

        #endregion

        #region Fields

        [Title("Base")]
        [SerializeField] private CharacterController _characterController;
        [Title("Movement Settings")]
        [SerializeField] private float _maxHorizontalSpeed;
        [SerializeField] private float _verticalVelocityModifier;
        [Title("Grounded settings")]
        [SerializeField] private GroundCheckType _groundCheckType;
        [ShowIf("@this._groundCheckType == GroundCheckType.Raycast")]
        [SerializeField] private Raycaster _groundRaycaster;
        [ShowIf("@this._groundCheckType == GroundCheckType.Trigger")]
        [SerializeField] private TriggerColliderEvents _groundTriggerEvents;
        [FoldoutGroup("Debug")]
        [SerializeField,DisplayAsString] private bool _isGrounded;

        private Vector3 _horizontalVelocity;
        private float _verticalVelocity;
        private float _gravity = -9.81f;
        private Vector3 _movementDirection = Vector3.forward;
        
        private List<MovementPriorityChanger> _movementPriorityChangers = new();
        
        #endregion

        #region Properties

        public float MaxHorizontalSpeed
        {
            get => _maxHorizontalSpeed;
            set => _maxHorizontalSpeed = value;
        }

        public bool IsGrounded
        {
            get => _isGrounded;
            set => _isGrounded = value;
        }

        public float Gravity 
        {
            get => _gravity;
            set => _gravity = value;
        }

        public Vector3 HorizontalVelocity { get => _horizontalVelocity; set => _horizontalVelocity = value; }
        public float VerticalVelocity { get => _verticalVelocity; set => _verticalVelocity = value; }
        public Vector3 NewMovementDirection { get => _movementDirection; set => _movementDirection = value; }

        #endregion

        #region Monobehaviour

        private void Update()
        {
            UpdateGroundedState();
            UpdateMovement();
        }

        #endregion
        
        #region MovementCore

        public void UpdateGroundedState()
        {
            _isGrounded =_groundCheckType switch
            {
                GroundCheckType.Raycast => _groundRaycaster?.IsHit() ?? false,
                GroundCheckType.Trigger => _groundTriggerEvents.Colliders.Count > 0,
                _ => _isGrounded
            };
        }

        public void SetInputMovementModifier(MovementPriorityChanger priorityChanger)
        {
            _movementPriorityChangers.Add(priorityChanger);
        }

        public Vector3 GetInputMovementModifier()
        {
            if (_movementPriorityChangers.Count == 0)
                return Vector3.zero;
    
            var highestPriorityChanger = _movementPriorityChangers
                .OrderByDescending(changer => changer.Priority)
                .First();
    
            return highestPriorityChanger.NewMovementDirection;
        }
        
        public void UpdateMovement()
        {
            var verticalVelocity = new Vector3(0, _verticalVelocity * Time.deltaTime * _verticalVelocityModifier, 0);
            _characterController.Move(_horizontalVelocity + verticalVelocity);
        }

        public void SetVerticalVelocity(float velocity)
        {
            _verticalVelocity = velocity;
        }

        public void AddVerticalImpulse(float impulse)
        {
            _verticalVelocity += impulse;
        }

        public float GetVerticalVelocity()
        {
            return _verticalVelocity;
        }

        #endregion
    }
}