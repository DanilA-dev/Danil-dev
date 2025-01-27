using System;

namespace D_Dev.UtilScripts.TimerSystem
{
    public abstract class BaseTimer
    {
       #region Fields

       protected float _initialTime;
       
       public Action OnTimerStart;
       public Action OnTimerEnd;

       #endregion

       #region Properties

       protected float Time { get; set; }
       public bool IsRunning { get; protected set; }
       public bool IsFinished { get; protected set; }
       public float Progress => Time / _initialTime;

       #endregion

       #region Construct

       public BaseTimer(float initialTime)
       {
          _initialTime = initialTime;
          IsRunning = false;
       }

       #endregion

       #region Public

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

       #endregion

       #region Abstract

       public abstract void Tick(float deltaTime);
       public abstract void Reset();

       #endregion
    }
}