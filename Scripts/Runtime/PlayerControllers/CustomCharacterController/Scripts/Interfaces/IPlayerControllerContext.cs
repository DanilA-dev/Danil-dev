namespace CustomCharacterController.Interfaces
{
    public interface IPlayerControllerContext
    {
        public IPlayerCameraController PlayerCameraController { get; }
        public IPlayerInputProvider PlayerInputProvider { get; }
        public IPlayerMovementCore PlayerMovementCore { get; }
    }
}