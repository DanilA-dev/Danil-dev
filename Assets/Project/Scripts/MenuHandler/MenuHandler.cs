using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.MenuHandler
{
    public class MenuHandler : MonoBehaviour
    {
        #region Fields

        [SerializeField] private bool _createMenusOnEnable;
        [SerializeField] private RectTransform _overlayCanvas;
        [SerializeField] private RectTransform _cameraCanvas;
        [SerializeField] private List<MenuInfo> _menuInfos = new();

        private Dictionary<MenuInfo,BaseMenu> _createdMenus = new();

        #endregion

        #region Monobehaviour

        private void OnEnable()
        {
            if(_createMenusOnEnable)
                CreateMenus();
        }

        #endregion

        #region Public

        public void CreateMenus()
        {
            if (_menuInfos.Count <= 0)
            {
                Debug.LogError($"No menu infos found");
                return;
            }

            foreach (var menuInfo in _menuInfos)
            {
                var menuParent = menuInfo.Canvas == 
                                 MenuInfo.CanvasType.Overlay ? _overlayCanvas : _cameraCanvas;
                var newMenu = Instantiate(menuInfo.MenuPrefab, menuParent);
                newMenu.gameObject.SetActive(false);
                _createdMenus.Add(menuInfo,newMenu);
            }
        }

        public void OpenMenu(MenuInfo menuInfo)
        {
            if(_createdMenus.TryGetValue(menuInfo, out var menu))
                menu.Open();
        }

        public void CloseMenu(MenuInfo menuInfo)
        {
            if(_createdMenus.TryGetValue(menuInfo, out var menu))
                menu.Close();
        }

        public void CloseAllMenus()
        {
            if(_createdMenus.Count <= 0)
                return;

            foreach (var keyValuePair in _createdMenus)
                keyValuePair.Value.Close();
        }

        #endregion
    }
}