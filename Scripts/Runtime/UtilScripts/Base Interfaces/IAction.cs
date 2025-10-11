
namespace D_dev
{
    public interface IAction
    {
        public bool IsFinished { get; }
        public void Execute();
        public void Undo();
    }
}
