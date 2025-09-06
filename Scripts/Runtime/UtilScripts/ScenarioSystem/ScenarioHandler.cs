using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_dev.Scripts.ScenarioSystem
{
    public class ScenarioHandler : MonoBehaviour
    {
        #region Fields

        [SerializeField] private bool _startScenarioOnStart;
        [SerializeField] private bool _saveScenarioState;
        [ShowIf(nameof(_startScenarioOnStart))]
        [SerializeField] private string _scenarioSaveID;
        [Title("Nodes")]
        [SerializeField] private BaseScenarioNode[] _scenarioNodes;

        private int _currentScenarioNodeIndex;
        
        #endregion

        #region Properties

        public bool IsFinished { get; private set; }
        public bool IsExecuting { get; private set; }

        #endregion

        #region Monobehaviour

        private void Start()
        {
            if(_startScenarioOnStart)
                StartScenario();
        }

        private void Update()
        {
            IsExecuting = _scenarioNodes.Length > 0 && _scenarioNodes.Any(n => n.IsExecuting);
            IsFinished = _scenarioNodes.Length > 0 && _scenarioNodes.All(n => n.IsFinished);
        }

        #endregion

        #region Public

        public void StartScenario()
        {
            if(IsExecuting || IsFinished)
                return;

            if (GetScenarioSavedState())
            {
                IsFinished = true;
                IsExecuting = true;
                return;
            }
            
            //TODO
            //if(_saveScenarioState && GetLastActiveSavedNode)
            
        }



        #endregion

        #region Private

        private bool GetScenarioSavedState()
        {
            return _saveScenarioState && PlayerPrefs.GetInt(_scenarioSaveID, 0) == 1;
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
            
        }

        private string GetLastSavedNode()
        {
            return "_lastActiveNode";
        }

        
        #endregion
    }
}