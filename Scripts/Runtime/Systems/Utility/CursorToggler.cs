using UnityEngine;
using UnityEngine.InputSystem;

namespace D_Dev.Utility
{
    public class CursorToggler : MonoBehaviour
    {
        #region Classes

        [System.Serializable]
        public class CursorState
        {
            [field: SerializeField] public CursorLockMode LockMode { get; set; }
            [field: SerializeField] public bool Visible { get; set; }
            
            public CursorState(CursorLockMode lockMode, bool visible)
            {
                LockMode = lockMode;
                Visible = visible;
            }
        }
        

        #endregion
        
        #region Fields

        [SerializeField] private int _setCursorStateIndexOnStart;
        [SerializeField] private CursorState[] _cursorStates;

        private bool _isStartPresetPending;

        #endregion

        #region Monobehavior

        private void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            _isStartPresetPending = true;
#else
            ApplyCursorStatePreset(_setCursorStateIndexOnStart);
#endif
        }

        private void Update()
        {
            if (!_isStartPresetPending)
                return;

            if (Pointer.current == null || !Pointer.current.press.wasPressedThisFrame)
                return;

            ApplyCursorStatePreset(_setCursorStateIndexOnStart);
        }

        #endregion

        #region Public

        public void ToggleCursor(CursorState cursorState)
        {
            Cursor.lockState = cursorState.LockMode;
            Cursor.visible = cursorState.Visible;
        }
        
        public void ApplyCursorStatePreset(int index)
        {
            _isStartPresetPending = false;

            if(_cursorStates.Length <= 0)
                return;
            
            var cursorState = _cursorStates[index];
            if (cursorState == null)
            {
                Debug.LogError($"[CursorToggler] Cursor state with index {index} is null");
                return;
            }
            ToggleCursor(cursorState);
        }

        #endregion
    }
}
