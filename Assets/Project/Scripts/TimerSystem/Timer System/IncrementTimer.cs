public class IncrementTimer : BaseTimer
{
    private float _targetTime;
    
    public IncrementTimer(float initialTime, float targetTime) : base(0)
    {
        _targetTime = targetTime;
    }

    public override void Tick(float deltaTime)
    {
        if (IsRunning && Time < _targetTime)
            Time += deltaTime;
        
        if(IsRunning && Time >= _targetTime)
            Stop();
    }

    public override void Reset() => Time = 0;
}