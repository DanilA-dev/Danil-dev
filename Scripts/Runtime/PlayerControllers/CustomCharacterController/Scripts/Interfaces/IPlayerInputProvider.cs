using System;
using UnityEngine;

namespace CustomCharacterController.Interfaces
{
    public interface IPlayerInputProvider
    {
        public event Action<Vector2> Move;
        public event Action<Vector2> Look;
        public event Action<bool> JumpPressed;
        public event Action<bool> SprintPressed;
        public event Action<bool> CrouchPressed;
        public event Action<bool> EscPressed;
        public event Action<bool> InteractPressed;
        public event Action<bool> RmbPressed;
        public event Action<bool> LmbPressed;
    }
}