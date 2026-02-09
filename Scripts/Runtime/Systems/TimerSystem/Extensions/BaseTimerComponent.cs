using System;
using D_Dev.PolymorphicValueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.TimerSystem
{
    public abstract class BaseTimerComponent : MonoBehaviour
    {
        #region Fields

        [SerializeField] protected bool _invokeOnStart;
        [SerializeReference] protected PolymorphicValue<float> _timeValue = new FloatConstantValue();

        [FoldoutGroup("Events")]
        public UnityEvent<float> OnTimerStart;
        [FoldoutGroup("Events")]
        public UnityEvent<float> OnTimerUpdate;
        [FoldoutGroup("Events")]
        public UnityEvent<float> OnTimerEnd;

        protected CountdownTimer _timer;
        protected Action _onTimerStartDelegate;
        protected Action _onTimerEndDelegate;
        protected Action<float> _onTimerProgressUpdateDelegate;
             
        #endregion

        #region Properties

        public bool InvokeOnStart
        {
            get => _invokeOnStart;
            set => _invokeOnStart = value;
        }

        public PolymorphicValue<float> TimeValue
        {
            get => _timeValue;
            set => _timeValue = value;
        }

        #endregion

        #region Monobehaviour

        protected virtual void Start()
        {
            _timer = new CountdownTimer(_timeValue.Value);
            
            _onTimerStartDelegate = () => OnTimerStart?.Invoke(_timeValue.Value);
            _onTimerEndDelegate = () => OnTimerEnd?.Invoke(_timer.RemainingTime);
            _onTimerProgressUpdateDelegate = (value) => OnTimerUpdate?.Invoke(value);
            
            _timer.OnTimerStart += _onTimerStartDelegate;
            _timer.OnTimerEnd += _onTimerEndDelegate;
            _timer.OnTimerProgressUpdate += _onTimerProgressUpdateDelegate;
            
            if (_invokeOnStart)
                StartTimer();
        }

        protected virtual void OnDestroy()
        {
            if (_timer != null)
            {
                _timer.OnTimerStart -= _onTimerStartDelegate;
                _timer.OnTimerEnd -= _onTimerEndDelegate;
                _timer.OnTimerProgressUpdate -= _onTimerProgressUpdateDelegate;
            }
        }

        #endregion

        #region Public

        public void UpdateTimer(float delta)
        {
            if (_timer != null)
                _timer.Tick(Time.deltaTime);
        }
        
        public void ResetTimer(float time) => _timer?.Reset(time);
        public void StartTimer() => _timer?.Start();
        public void StopTimer() => _timer?.Stop();
        public void PauseTimer() => _timer?.Pause();

        #endregion
    }
}
