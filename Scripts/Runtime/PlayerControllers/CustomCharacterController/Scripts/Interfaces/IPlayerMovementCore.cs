using System;
using CustomCharacterController.Core;
using UnityEngine;

namespace CustomCharacterController.Interfaces
{
    public interface IPlayerMovementCore
    {
        public float MaxHorizontalSpeed { get; set; }
        public bool IsGrounded { get; }
        public float Gravity { get; set; }
        public Vector3 HorizontalVelocity { get; set; }
        public float VerticalVelocity { get; set; }
        public void SetInputMovementModifier(MovementPriorityChanger priorityChanger);
        public Vector3 GetInputMovementModifier();
        public void UpdateGroundedState();
        public void UpdateMovement();
        public void SetVerticalVelocity(float velocity);
        public void AddVerticalImpulse(float impulse);
        public float GetVerticalVelocity();
    }
}