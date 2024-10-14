using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class FocusScrollItem : MonoBehaviour
    {
        [FoldoutGroup("Events")] 
        public UnityEvent OnScrollFocus;
        [FoldoutGroup("Events")]
        public UnityEvent OnScrollResetFocus;

        public bool IsFocused { get; private set; }

        public void SetFocus(bool value)
        {
            IsFocused = value;
            
            if(IsFocused)
                OnScrollFocus?.Invoke();
            else
                OnScrollResetFocus?.Invoke();
        }
    }
}
