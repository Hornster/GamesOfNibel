using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.GUI.Menu.Transitions
{
    /// <summary>
    /// Manages passes between menu parts. Should be positioned in parent gameobject of all available menus.
    /// </summary>
    public class MenuTransitionManager : MonoBehaviour
    {
        /// <summary>
        /// All available menus.
        /// </summary>
        private Dictionary<MenuType, MenuVisibilityController> _availableMenus = new Dictionary<MenuType, MenuVisibilityController>();
        /// <summary>
        /// How long does it take to fade in or fade out.
        /// </summary>
        private float _transitionTime = 0.5f;
        /// <summary>
        /// Reference to the gameobject that has menus as children.
        /// </summary>
        [Tooltip("Gameobject that has menus as children in it.")]
        private GameObject _menusParent;

        private MenuTransitionController _transitionController;
        private void Start()
        {
            var foundMenus = _menusParent.GetComponentsInChildren<MenuVisibilityController>();
            foreach (var menu in foundMenus)
            {
                _availableMenus.Add(menu.MenuType, menu);
            }

        }
        /// <summary>
        /// Performs transition from given menu to given menu.
        /// </summary>
        /// <param name="fromMenu"></param>
        /// <param name="toMenu"></param>
        public void PerformTransition(MenuType fromMenu, MenuType toMenu)
        {
            //TODO get the main gameobjects for the menus passed above. CanvasGroups can allow you to
            //TODO hide entire menus and disable raycasting for them.
            //TODO start fade out transition, when it finishes toggle both menus and start fade in transition.
            //Fade out current menu.
            StartCoroutine(FadeOut(_transitionController));
            //Get and change visibility of the current and next menu.
            var currentMenu = GetVisibilityControllerForMenu(fromMenu);
            currentMenu?.HideMenu();

            var nextMenu = GetVisibilityControllerForMenu(toMenu);
            nextMenu?.ShowMenu();
            //Fade in the next menu.
            StartCoroutine(FadeIn(_transitionController));

        }
        /// <summary>
        /// Tries to retrieve given menu type. If fails, returns null.
        /// </summary>
        /// <param name="menuType"></param>
        /// <returns></returns>
        private MenuVisibilityController GetVisibilityControllerForMenu(MenuType menuType)
        {
            if (_availableMenus.TryGetValue(menuType, out var menu))
            {
                return menu;
            }

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeIn(MenuTransitionController transitionController)
        {
            //Play the fade in animation
            float fadeInDuration = transitionController.FadeIn();
            yield return new WaitForSeconds(fadeInDuration);
        }
        private IEnumerator FadeOut(MenuTransitionController transitionController)
        {
            float fadeOutDuration = transitionController.FadeOut();
            yield return new WaitForSeconds(fadeOutDuration);
        }
    }

    
}