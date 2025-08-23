using UnityEngine;

namespace CustomCharacterController.Interfaces
{
    public interface IPlayerCameraController : IPlayerContextDepender
    {
        public Transform CameraRoot { get; }
        public Vector3 CameraForward { get; }
        public Vector3 CameraRight { get; }
        public void LockCamera();
        public void UnlockCamera();
    }
}