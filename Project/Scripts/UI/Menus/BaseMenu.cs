using UnityEngine;

namespace UI
{
    public enum MenuType
    {
        None = 0,
        Menu = 1,
        Core = 2,
        Pause = 3
    }
    
    public abstract class BaseMenu : MonoBehaviour
    {
        public abstract MenuType Type { get; }
    }
}