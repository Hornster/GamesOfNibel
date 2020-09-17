﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common.CustomCollections;
using Assets.Scripts.Common.CustomEvents;
using Assets.Scripts.Common.Data;
using Assets.Scripts.Common.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GUI.Menu.Transitions
{
    /// <summary>
    /// Manages passes between menu parts. Should be positioned in parent gameobject of all available menus.
    /// </summary>
    public class MenuTransitionManager : MonoBehaviour
    {
        /// <summary>
        /// Reference to the gameobject that has menus as children.
        /// </summary>
        [Header("Menu transition")]
        [Tooltip("Gameobject that has menus as children in it.")]
        [SerializeField]
        private GameObject _menusParent;
        [Tooltip("Which menu shall show up as first?")]
        [SerializeField]
        private MenuType _startingMenu = MenuType.WelcomeMenu;
        [Tooltip("Used to send info about new set of controls found in the freshly selected menu.")]
        [SerializeField]
        private CustomControlsFinderUnityEvent _reportFoundControlsUponTransition;

        [Header("Scene transition")] 
        [Tooltip("The time it takes for entire scene to fade away. Shall be equal to the length of the fade out animation.")]
        [SerializeField]
        private float _sceneFadeOutTime = 1f;
        [Tooltip("What is the name of the trigger that causes the fade out scene animation to play.")]
        [SerializeField] private string _sceneFadeOutTrigger = "fadeOut";
        [Tooltip("Reference to the animator that manages entire scene transitions.")]
        [SerializeField]
        private Animator _sceneTransitionAnimator;
        /// <summary>
        /// All available menus.
        /// </summary>
        private Dictionary<MenuType, MenuTransition> _availableMenus = new Dictionary<MenuType, MenuTransition>();
        /// <summary>
        /// Stores the most recent transition.
        /// </summary>
        private MenuTransitionStepHistory _menuTransitionsHistory;
        /// <summary>
        /// Should the most recent transition be remembered so it can be optionally reversed later?
        /// </summary>
        private bool _rememberStep = true;
        private void Start()
        {
            var foundMenus = _menusParent.GetComponentsInChildren<MenuTransition>();
            foreach (var menu in foundMenus)
            {
                _availableMenus.Add(menu.MenuType, menu);
            }

            PerformTransition(_startingMenu, _startingMenu);
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
            //Get and change visibility of the current and next menu.
            var currentMenu = GetVisibilityControllerForMenu(fromMenu);
            currentMenu?.HideMenu();

            var nextMenu = GetVisibilityControllerForMenu(toMenu);
            nextMenu?.ShowMenu();

            if (_rememberStep)
            {
                _menuTransitionsHistory = new MenuTransitionStepHistory()
                {
                    TransitionFrom = fromMenu,
                    TransitionTo = toMenu
                };
            }
            
            ReportNewControls(nextMenu);
            //Fade out current menu.
            StartCoroutine(FadeOut(currentMenu));
            
            //Fade in the next menu.
            StartCoroutine(FadeIn(nextMenu));

        }
        /// <summary>
        /// Performs the fade out scene transition.
        /// </summary>
        public void PerformSceneTransition(string scenePath)
        {
            StartCoroutine(SceneFadeOut(scenePath));
        }
        /// <summary>
        /// Causes the manager to revert the last made transition. Reverse transition will not be remembered.
        /// </summary>
        public void RevertLastTransition()
        {
            //Reverse the last made transition but do not remember doing this.
            _rememberStep = false;
            PerformTransition(_menuTransitionsHistory.TransitionTo, _menuTransitionsHistory.TransitionFrom);
            _rememberStep = true;
        }
        /// <summary>
        /// Sends info about found controls for new menu to all listening objects. If any controls were found, that is.
        /// </summary>
        /// <param name="newMenu"></param>
        private void ReportNewControls(MenuTransition newMenu)
        {
            var foundControls = newMenu?.GetComponentInChildren<CustomControlsFinder>();

            if (foundControls != null)
            {
                _reportFoundControlsUponTransition?.Invoke(foundControls);
            }
        }
        /// <summary>
        /// Tries to retrieve given menu type. If fails, returns null.
        /// </summary>
        /// <param name="menuType"></param>
        /// <returns></returns>
        private MenuTransition GetVisibilityControllerForMenu(MenuType menuType)
        {
            if (_availableMenus.TryGetValue(menuType, out var menu))
            {
                return menu;
            }

            return null;
        }
        /// <summary>
        /// Manages the fade in sequence of the next menu.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeIn(MenuTransition transition)
        {
            if (transition != null)
            {
                //Play the fade in animation
                float fadeInDuration = transition.FadeIn();
                yield return new WaitForSeconds(fadeInDuration);
                transition.ShowMenu();
            }
        }
        /// <summary>
        /// Manages the fade out sequence of the current menu.
        /// </summary>
        /// <param name="transition"></param>
        /// <returns></returns>
        private IEnumerator FadeOut(MenuTransition transition)
        {
            if (transition != null)
            {
                float fadeOutDuration = transition.FadeOut();
                yield return new WaitForSeconds(fadeOutDuration);
                transition.HideMenu();
            }
        }
        /// <summary>
        /// Performs the scene fade out.
        /// </summary>
        private IEnumerator SceneFadeOut(string scenePath)
        {
            _sceneTransitionAnimator.SetTrigger(_sceneFadeOutTrigger);

            yield return new WaitForSeconds(_sceneFadeOutTime);

            SceneManager.LoadScene(scenePath);
        }
    }

    
}