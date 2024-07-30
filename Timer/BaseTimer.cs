using System;


public abstract class BaseTimer
{
   protected float _initialTime;
   
   protected float Time { get; set; }
   public bool IsRunning { get; protected set; }
   public bool IsFinished { get; protected set; }
   public float Progress => Time / _initialTime;

   public Action OnTimerStart;
   public Action OnTimerEnd;
   

   public BaseTimer(float initialTime)
   {
      _initialTime = initialTime;
      IsRunning = false;
   }

   public void Start()
   {
      Time = _initialTime;
      if (!IsRunning)
      {
         IsFinished = false;
         IsRunning = true;
         OnTimerStart?.Invoke();
      }
         
   }
   
   public void Stop()
   {
      if (IsRunning)
      {
         IsFinished = true;
         IsRunning = false;
         OnTimerEnd?.Invoke();
      }
   }

   public void Resume() => IsRunning = true;
   public void Pause() => IsRunning = false;

   public abstract void Tick(float deltaTime);
   public abstract void Reset();
}