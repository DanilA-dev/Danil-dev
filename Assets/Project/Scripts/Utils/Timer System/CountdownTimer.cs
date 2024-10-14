public class CountdownTimer : BaseTimer
{
    public float RemainingTime => Time;
    
    public CountdownTimer(float initialTime) : base(initialTime) {}

    public override void Tick(float deltaTime)
    {
        if (IsRunning && Time > 0)
            Time -= deltaTime;
      
        if(IsRunning && Time <= 0)
            Stop();
    }

    public override void Reset() => Time = _initialTime;

    public void Reset(float newInitTime)
    {
        _initialTime = newInitTime;
        Reset();
    }
}