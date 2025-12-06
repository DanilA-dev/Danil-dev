using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.TimerSystem
{
    public class TimerComponent : MonoBehaviour
    {
        #region Fields

        [SerializeField] private bool _invokeOnStart;
        [SerializeField] private float _timeValue;

        [FoldoutGroup("Events")]
        public UnityEvent<float> OnTimerStart;
        [FoldoutGroup("Events")]
        public UnityEvent<float> OnTimerUpdate;
        [FoldoutGroup("Events")]
        public UnityEvent<float> OnTimerEnd;

        private CountdownTimer _timer;
            
        #endregion

        #region Properties

        public bool InvokeOnStart
        {
            get => _invokeOnStart;
            set => _invokeOnStart = value;
        }

        public float TimeValue
        {
            get => _timeValue;
            set => _timeValue = value;
        }

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _timer = new CountdownTimer(_timeValue);
            _timer.OnTimerStart += () => OnTimerStart?.Invoke(_timeValue);
            _timer.OnTimerEnd += () => OnTimerEnd?.Invoke(_timer.RemainingTime);
            _timer.OnTimerProgressUpdate += (value) => OnTimerUpdate?.Invoke(value);
        }

        private void Start()
        {
            if (_invokeOnStart)
                StartTimer();
        }

        private void OnDestroy()
        {
            _timer.OnTimerStart -= () => OnTimerStart?.Invoke(_timeValue);
            _timer.OnTimerEnd -= () => OnTimerEnd?.Invoke(_timeValue);
            _timer.OnTimerProgressUpdate -= (value) => OnTimerUpdate?.Invoke(value);
        }

        private void Update() => _timer?.Tick(Time.deltaTime);

        #endregion

        #region Public

        public void ResetTimer(float time) => _timer?.Reset(time);
        public void StartTimer() => _timer?.Start();
        public void StopTimer() => _timer?.Stop();
        public void PauseTimer() => _timer?.Pause();

        #endregion
    }
}
