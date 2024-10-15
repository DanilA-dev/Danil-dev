using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName = "Data/Info/MenuInfo")]
    public class MenuInfo : ScriptableObject
    {
        [field:SerializeField] public BaseMenu MenuPrefab;
    }
}