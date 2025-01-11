namespace D_Dev.UtilScripts.State_Machine
{
    public interface IState
    {
        public void OnEnter();
        public void OnUpdate();
        public void OnFixedUpdate();
        public void OnExit();
    }
}