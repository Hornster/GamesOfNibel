using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.GUI.Menu.Transitions
{
    /// <summary>
    /// Controls visibility of single menu.
    /// </summary>
    public class MenuVisibilityController
    {
        [SerializeField] private CanvasGroup _menuCanvasGroup;

        [SerializeField]
        [Tooltip("What type of menu is connected with this controller.")]
        private MenuType _menuType;

        public MenuType MenuType => _menuType;
        /// <summary>
        /// Makes the menu invisible, disables raycasts blocking and interactability for the menu.
        /// </summary>
        public void HideMenu()
        {
            _menuCanvasGroup.alpha = 0.0f;
            _menuCanvasGroup.blocksRaycasts = false;
            _menuCanvasGroup.interactable = false;
        }
        /// <summary>
        /// Makes the menu visible, enables raycasts and interactability for the menu.
        /// </summary>
        public void ShowMenu()
        {
            _menuCanvasGroup.alpha = 1.0f;
            _menuCanvasGroup.blocksRaycasts = true;
            _menuCanvasGroup.interactable = true;
        }
    }
}