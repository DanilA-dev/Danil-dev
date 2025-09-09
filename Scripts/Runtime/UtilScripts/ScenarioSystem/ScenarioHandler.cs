using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_dev.Scripts.ScenarioSystem
{
    public class ScenarioHandler : MonoBehaviour
    {
        #region Fields

        [SerializeField] private bool _startScenarioOnStart;
        [SerializeField] private bool _saveScenarioState;
        [ShowIf(nameof(_startScenarioOnStart))]
        [SerializeField] private string _scenarioSaveID;
        
        [Title("Pause System")]
        [SerializeField] private bool _isPaused;
        
        [Title("Safety")]
        [SerializeField] private float _nodeTimeoutSeconds = 10;
        [SerializeField] private bool _enableTimeoutProtection = true;
        
        [Title("Executable Nodes")]
        [SerializeField] private BaseScenarioExecutableNode[] _scenarioNodes;
        
        [Title("Break Nodes")]
        [SerializeField] private BaseScenarioBreakNode[] _scenarioBreakers;

        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent _onScenarioStarted;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent _onScenarioFinished;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent _onScenarioPaused;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent _onScenarioResumed;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent<int> _onNodeStarted;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent<int> _onNodeFinished;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent<int> _onNodeTimeout;
        [FoldoutGroup("Events")]
        [SerializeField] private UnityEvent<BaseScenarioBreakNode> _onScenarioBroken;

        private int _currentScenarioNodeIndex;
        private float _currentNodeStartTime;
        private bool _scenarioStarted;
        
        #endregion

        #region Properties

        public bool IsFinished { get; private set; }
        public bool IsPaused => _isPaused;
        public int CurrentNodeIndex => _currentScenarioNodeIndex;
        public BaseScenarioExecutableNode CurrentNode => 
            _scenarioNodes != null && _currentScenarioNodeIndex < _scenarioNodes.Length 
                ? _scenarioNodes[_currentScenarioNodeIndex] 
                : null;

        #endregion

        #region Monobehaviour

        private void Start()
        {
            if(_startScenarioOnStart)
                StartScenario();
        }

        private void Update()
        {
            if (!IsFinished && !_isPaused)
                ExecuteScenario();
        }

        #endregion

        #region Public

        public void StartScenario()
        {
            if(IsFinished || _scenarioStarted)
                return;

            if (_scenarioNodes == null || _scenarioNodes.Length == 0)
                return;

            if (GetScenarioSavedState())
            {
                IsFinished = true;
                return;
            }
            
            _currentScenarioNodeIndex = GetLastActiveNodeState() 
                ? GetLastActiveSavedNode() 
                : 0;
                
            _currentScenarioNodeIndex = Mathf.Clamp(_currentScenarioNodeIndex, 0, _scenarioNodes.Length - 1);
                
            _scenarioStarted = true;
            _onScenarioStarted?.Invoke();
        }

        public void PauseScenario()
        {
            if (!_isPaused && !IsFinished)
            {
                _isPaused = true;
                _onScenarioPaused?.Invoke();
            }
        }

        public void ResumeScenario()
        {
            if (_isPaused && !IsFinished)
            {
                _isPaused = false;
                _onScenarioResumed?.Invoke();
            }
        }

        public void StopScenario()
        {
            IsFinished = true;
            _scenarioStarted = false;
            _onScenarioFinished?.Invoke();
            
            if (_saveScenarioState)
                SaveScenarioState();
        }

        public void ResetBreakers()
        {
            if (_scenarioBreakers == null)
                return;
            
            foreach (var breaker in _scenarioBreakers)
            {
                breaker?.Reset();
            }
        }

        public void SetBreakersActive(bool active)
        {
            if (_scenarioBreakers == null)
                return;
            
            foreach (var breaker in _scenarioBreakers)
            {
                breaker?.SetActive(active);
            }
        }

        #endregion

        #region Private

        private void ExecuteScenario()
        {
            if (CheckScenarioBreakers())
                return;
                
            if (_scenarioNodes == null || _scenarioNodes.Length == 0)
            {
                FinishScenario();
                return;
            }
            
            if (_currentScenarioNodeIndex >= _scenarioNodes.Length)
            {
                FinishScenario();
                return;
            }

            BaseScenarioExecutableNode currentNode = _scenarioNodes[_currentScenarioNodeIndex];
            
            if (currentNode == null)
            {
                MoveToNextNode();
                return;
            }

            if (currentNode.IsFinished)
            {
                OnNodeFinished();
                MoveToNextNode();
                return;
            }

            if (_enableTimeoutProtection && CheckNodeTimeout())
            {
                _onNodeTimeout?.Invoke(_currentScenarioNodeIndex);
                
                ForceFinishCurrentNode();
                MoveToNextNode();
                return;
            }

            if (_currentNodeStartTime == 0)
                StartCurrentNode();

            currentNode.Execute();
        }

        private void StartCurrentNode()
        {
            _currentNodeStartTime = Time.time;
            _onNodeStarted?.Invoke(_currentScenarioNodeIndex);
            
            if (_saveScenarioState)
                SaveLastActiveSavedNode();
        }

        private void OnNodeFinished()
        {
            _onNodeFinished?.Invoke(_currentScenarioNodeIndex);
            _currentNodeStartTime = 0;
        }

        private void MoveToNextNode()
        {
            _currentScenarioNodeIndex++;
            _currentNodeStartTime = 0;
            
            if (_saveScenarioState)
                SaveLastActiveSavedNode();
        }

        private void FinishScenario()
        {
            if (!IsFinished)
            {
                IsFinished = true;
                _scenarioStarted = false;
                _onScenarioFinished?.Invoke();
                
                if (_saveScenarioState)
                    SaveScenarioState();
            }
        }

        private bool CheckNodeTimeout()
        {
            return _currentNodeStartTime > 0 && 
                   Time.time - _currentNodeStartTime > _nodeTimeoutSeconds;
        }

        private void ForceFinishCurrentNode()
        {
            BaseScenarioExecutableNode currentNode = _scenarioNodes[_currentScenarioNodeIndex];
            currentNode?.ForceFinish();
        }

        private bool CheckScenarioBreakers()
        {
            if (_scenarioBreakers == null || _scenarioBreakers.Length == 0)
                return false;

            foreach (var breaker in _scenarioBreakers)
            {
                if (breaker == null)
                    continue;

                if (breaker.ShouldBreakScenario())
                {
                    Debug.Log($"[ScenarioHandler] Scenario broken by {breaker.name}");
                    
                    _onScenarioBroken?.Invoke(breaker);
                    
                    FinishScenario();
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Saves

        private bool GetScenarioSavedState()
        {
            return _saveScenarioState && PlayerPrefs.GetInt(_scenarioSaveID, 0) == 1;
        }

        private bool GetLastActiveNodeState()
        {
            return _saveScenarioState && PlayerPrefs.HasKey(GetLastSavedNode());
        }
        
        private int GetLastActiveSavedNode()
        {
            _currentScenarioNodeIndex = PlayerPrefs.GetInt(GetLastSavedNode(), 0);
            return _currentScenarioNodeIndex;
        }

        private void SaveScenarioState()
        {
            PlayerPrefs.SetInt(_scenarioSaveID, IsFinished? 1 : 0);
        }

        private void SaveLastActiveSavedNode()
        {
            PlayerPrefs.SetInt(GetLastSavedNode(), _currentScenarioNodeIndex);
        }

        private string GetLastSavedNode()
        {
            return $"{_scenarioSaveID}_lastActiveNode";
        }

        
        #endregion
    }
}