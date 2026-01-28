using System;
using UnityEngine;

namespace D_Dev.StateMachineBehaviour
{
    public class UIStateDebugger : MonoBehaviour
    {
        #region Fields

        [SerializeField] private StateMachineBehaviour _stateMachineBehaviour;
        [SerializeField] private TextMesh _text;

        private Camera _camera;
        
        #endregion
        
        #region Monobehaviour

        private void OnEnable()
        {
            _camera = Camera.main;
            _stateMachineBehaviour.OnAnyStateEnter.AddListener(OnStateEnter);
        }
        private void OnDisable() => _stateMachineBehaviour.OnAnyStateEnter.RemoveListener(OnStateEnter);

        private void LateUpdate() => transform.LookAt(_camera.transform.position);

        #endregion

        #region Listeners
        private void OnStateEnter(string state) => _text.text = state;

        #endregion
    }
}
