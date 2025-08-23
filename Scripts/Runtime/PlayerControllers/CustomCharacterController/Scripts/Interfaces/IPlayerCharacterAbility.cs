namespace CustomCharacterController.Interfaces
{
    public interface IPlayerCharacterAbility : IPlayerContextDepender
    {
        public bool IsActive { get; set; }
        public bool IsExecuting { get; }
        public void TickUpdate();
        public void TickFixedUpdate();
        public void TickLateUpdate();
    }
}