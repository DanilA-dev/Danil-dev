using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using D_dev.ActionGroup;
using D_dev;

namespace D_Dev.ActionFlowController
{
    public class ActionFlowController : MonoBehaviour
    {
        #region Fields

        [SerializeField] private bool _executeOnStart;
        [SerializeField] private bool _saveState;
        [ShowIf(nameof(_saveState))]
        [SerializeField] private string _saveID;
        [Space]
        [SerializeReference] private ActionGroup[] _actionGroups;
        [SerializeReference] private ICondition[] _breakConditions;
        [Space]
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent _onStarted;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent _onFinished;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent _onPaused;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent _onResumed;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent<int> _onGroupStarted;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent<int> _onGroupFinished;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent<int> _onGroupTimeout;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent _onBroken;
        [PropertySpace(10)]
        [SerializeField, ReadOnly] private bool _isPaused;
        [SerializeField, ReadOnly] private bool _isFinished;

        private int _currentGroupIndex;
        private float _currentGroupStartTime;
        private bool _started;

        #endregion

        #region Properties

        public bool IsFinished => _isFinished;
        public bool IsPaused => _isPaused;
        public int CurrentGroupIndex => _currentGroupIndex;
        public ActionGroup CurrentGroup =>
            _actionGroups != null && _currentGroupIndex < _actionGroups.Length
                ? _actionGroups[_currentGroupIndex]
                : null;

        #endregion

        #region Monobehaviour

        private void Start()
        {
            if (_executeOnStart)
                StartExecution();
        }

        private void Update()
        {
            if (!IsFinished && !_isPaused)
                Execute();
        }

        #endregion

        #region Public

        public void StartExecution()
        {
            if (IsFinished || _started)
                return;

            if (_actionGroups == null || _actionGroups.Length == 0)
                return;

            if (GetSavedState())
            {
                _isFinished = true;
                return;
            }

            _currentGroupIndex = GetLastActiveState()
                ? GetLastSavedGroup()
                : 0;

            _currentGroupIndex = Mathf.Clamp(_currentGroupIndex, 0, _actionGroups.Length - 1);

            _started = true;
            _onStarted?.Invoke();
        }

        public void Pause()
        {
            if (!_isPaused && !IsFinished)
            {
                _isPaused = true;
                _onPaused?.Invoke();
            }
        }

        public void Resume()
        {
            if (_isPaused && !IsFinished)
            {
                _isPaused = false;
                _onResumed?.Invoke();
            }
        }

        public void Stop()
        {
            _isFinished = true;
            _started = false;
            _onFinished?.Invoke();

            if (_saveState)
                SaveState();
        }

        public void ResetBreakers()
        {
            if (_breakConditions == null)
                return;

            foreach (var condition in _breakConditions)
                condition?.Reset();
        }

        #endregion

        #region Private

        private void Execute()
        {
            if (!_started)
                return;

            if (CheckBreakers())
                return;

            if (_actionGroups == null || _actionGroups.Length == 0)
            {
                Finish();
                return;
            }

            if (_currentGroupIndex >= _actionGroups.Length)
            {
                Finish();
                return;
            }

            ActionGroup currentGroup = _actionGroups[_currentGroupIndex];

            if (currentGroup == null)
            {
                MoveToNext();
                return;
            }

            if (currentGroup.IsCompleted)
            {
                OnGroupFinished();
                MoveToNext();
                return;
            }

            if (_currentGroupStartTime == 0)
                StartCurrent();

            currentGroup.ExecuteActions();
        }

        private void StartCurrent()
        {
            _currentGroupStartTime = Time.time;
            _onGroupStarted?.Invoke(_currentGroupIndex);

            if (_saveState)
                SaveLastActive();
        }

        private void OnGroupFinished()
        {
            _onGroupFinished?.Invoke(_currentGroupIndex);
            _currentGroupStartTime = 0;
        }

        private void MoveToNext()
        {
            _currentGroupIndex++;
            _currentGroupStartTime = 0;

            if (_saveState)
                SaveLastActive();
        }

        private void Finish()
        {
            if (!IsFinished)
            {
                _isFinished = true;
                _started = false;
                _onFinished?.Invoke();

                if (_saveState)
                    SaveState();
            }
        }

        private bool CheckBreakers()
        {
            if (_breakConditions == null || _breakConditions.Length == 0)
                return false;

            foreach (var condition in _breakConditions)
            {
                if (condition == null)
                    continue;

                if (condition.IsConditionMet())
                {
                    Debug.Log($"[ActionExecuter] Broken by condition");

                    _onBroken?.Invoke();

                    Finish();
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Saves

        private bool GetSavedState()
        {
            return _saveState && PlayerPrefs.GetInt(_saveID, 0) == 1;
        }

        private bool GetLastActiveState()
        {
            return _saveState && PlayerPrefs.HasKey(GetLastSaved());
        }

        private int GetLastSavedGroup()
        {
            _currentGroupIndex = PlayerPrefs.GetInt(GetLastSaved(), 0);
            return _currentGroupIndex;
        }

        private void SaveState()
        {
            PlayerPrefs.SetInt(_saveID, IsFinished ? 1 : 0);
        }

        private void SaveLastActive()
        {
            PlayerPrefs.SetInt(GetLastSaved(), _currentGroupIndex);
        }

        private string GetLastSaved()
        {
            return $"{_saveID}_lastActive";
        }

        #endregion
    }
}
