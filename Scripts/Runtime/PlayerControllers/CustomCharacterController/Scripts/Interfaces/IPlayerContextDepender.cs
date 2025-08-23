namespace CustomCharacterController.Interfaces
{
    public interface IPlayerContextDepender
    {
        public bool IsInitialized { get; }
        public void Init(IPlayerControllerContext playerControllerContext);
    }
}