using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace D_Dev.ScenarioSystem
{
    public class ScenarioHandler : MonoBehaviour
    {
        #region Fields

        [SerializeField] private bool _startScenarioOnStart;
        [SerializeField] private bool _saveScenarioState;
        [ShowIf(nameof(_saveScenarioState))]
        [SerializeField] private string _scenarioSaveID;
        
        [SerializeField, ReadOnly] private bool _isPaused;
        [SerializeField, ReadOnly] private bool _isFinished;

        [SerializeField] private bool _finishAllNodesOnBreak = true;
        [SerializeField] private float _nodeTimeoutSeconds = 10;
        [SerializeField] private bool _enableTimeoutProtection = true;
       

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

        private BaseScenarioExecutableNode[] _scenarioExecutableNodes;
        private BaseScenarioBreakNode[] _scenarioBreakNodes;
        
        private int _currentScenarioNodeIndex;
        private float _currentNodeStartTime;
        private bool _scenarioStarted;
        
        #endregion

        #region Properties

        public bool IsFinished { get => _isFinished; }
        public bool IsPaused => _isPaused;
        public int CurrentNodeIndex => _currentScenarioNodeIndex;
        public BaseScenarioExecutableNode CurrentNode => 
            _scenarioExecutableNodes != null && _currentScenarioNodeIndex < _scenarioExecutableNodes.Length 
                ? _scenarioExecutableNodes[_currentScenarioNodeIndex] 
                : null;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _scenarioExecutableNodes = GetComponentsInChildren<BaseScenarioExecutableNode>();
            _scenarioBreakNodes = GetComponentsInChildren<BaseScenarioBreakNode>();
        }

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

            if (_scenarioExecutableNodes == null || _scenarioExecutableNodes.Length == 0)
                return;

            if (GetScenarioSavedState())
            {
                _isFinished = true;
                return;
            }
            
            _currentScenarioNodeIndex = GetLastActiveNodeState() 
                ? GetLastActiveSavedNode() 
                : 0;
                
            _currentScenarioNodeIndex = Mathf.Clamp(_currentScenarioNodeIndex, 0, _scenarioExecutableNodes.Length - 1);
                
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
            _isFinished = true;
            _scenarioStarted = false;
            _onScenarioFinished?.Invoke();
            
            if (_saveScenarioState)
                SaveScenarioState();
        }

        public void ResetBreakers()
        {
            if (_scenarioBreakNodes == null)
                return;
            
            foreach (var breaker in _scenarioBreakNodes)
                breaker?.Reset();
        }

        public void SetBreakersActive(bool active)
        {
            if (_scenarioBreakNodes == null)
                return;
            
            foreach (var breaker in _scenarioBreakNodes)
                breaker?.SetActive(active);
        }

        #endregion

        #region Private

        private void ExecuteScenario()
        {
            if(!_scenarioStarted)
                return;
            
            if (CheckScenarioBreakers())
                return;
                
            if (_scenarioExecutableNodes == null || _scenarioExecutableNodes.Length == 0)
            {
                FinishScenario();
                return;
            }
            
            if (_currentScenarioNodeIndex >= _scenarioExecutableNodes.Length)
            {
                FinishScenario();
                return;
            }

            BaseScenarioExecutableNode currentNode = _scenarioExecutableNodes[_currentScenarioNodeIndex];
            
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
                _isFinished = true;
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
            BaseScenarioExecutableNode currentNode = _scenarioExecutableNodes[_currentScenarioNodeIndex];
            currentNode?.ForceFinish();
        }

        private bool CheckScenarioBreakers()
        {
            if (_scenarioBreakNodes == null || _scenarioBreakNodes.Length == 0)
                return false;

            foreach (var breaker in _scenarioBreakNodes)
            {
                if (breaker == null)
                    continue;

                if (breaker.ShouldBreakScenario())
                {
                    Debug.Log($"[ScenarioHandler] Scenario broken by {breaker.name}");
                    
                    _onScenarioBroken?.Invoke(breaker);
                    
                    FinishScenario();
                    if (_finishAllNodesOnBreak && _scenarioExecutableNodes != null)
                    {
                        foreach (var executableNode in _scenarioExecutableNodes)
                        {
                            executableNode?.Execute();
                            executableNode?.ForceFinish();
                        }
                    }
                    
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
