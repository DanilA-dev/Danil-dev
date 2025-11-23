using UnityEngine;

namespace D_Dev.MenuHandler
{
    [CreateAssetMenu(menuName = "D-Dev/Info/MenuInfo")]
    public class MenuInfo : ScriptableObject
    {
        public enum CanvasType
        {
            Overlay = 0,
            Camera = 1
        }

        [field: SerializeField] public CanvasType Canvas { get; private set; }
        [field: SerializeField] public BaseMenu MenuPrefab { get; private set; }
        [field: SerializeField] public bool OpenOnCreate { get; private set; }
    }
}
