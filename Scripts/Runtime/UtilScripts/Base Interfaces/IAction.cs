
namespace D_dev
{
    public interface IAction
    {
        void Execute();
        bool IsFinished { get; }
    }
}
