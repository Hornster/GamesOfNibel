using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.CustomEvents;
using Assets.Scripts.Game.Common.Data;
using Assets.Scripts.Game.Common.Data.ScriptableObjects.ScenePassData;
using Assets.Scripts.Game.Common.Data.ScriptableObjects.Transitions;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.MapInitialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game.GUI.Menu.Transitions
{
    /// <summary>
    /// Manages passes between menu parts. Should be positioned in parent gameobject of all available menus.
    /// </summary>
    public class MenuTransitionManager : MonoBehaviour
    {
        [Header("Required references")] 
        [SerializeField]
        protected BackwardsTransitions _backwardsTransitions;
        [Tooltip("Holds info about what menu shall be loaded upon entering main menu.")]
        [SerializeField]
        protected ReturnFromSceneToMenuSO _startingMenu;
        [Tooltip("Prepares scene data for loaded scene.")]
        [SerializeField]
        private SceneDataInitializer _sceneDataInitializer;
        /// <summary>
        /// Reference to the gameobject that has menus as children.
        /// </summary>
        [Header("Menu transition")]
        [Tooltip("Gameobject that has menus as children in it.")]
        [SerializeField]
        protected GameObject _menusParent;
        [Tooltip("Used to send info about new set of controls found in the freshly selected menu.")]
        [SerializeField]
        protected CustomControlsFinderUnityEvent _reportFoundControlsUponTransition;
        /// <summary>
        /// Called when the ingame menu is turned off.
        /// </summary>
        [SerializeField]
        protected UnityEvent _onInGameMenuTurnedOff;


        [Header("Scene transition")] 
        [Tooltip("The time it takes for entire scene to fade away. Shall be equal to the length of the fade out animation.")]
        [SerializeField]
        protected float _sceneFadeOutTime = 1f;
        [Tooltip("What is the name of the trigger that causes the fade out scene animation to play.")]
        [SerializeField] protected string _sceneFadeOutTrigger = "FadeOut";
        [Tooltip("Reference to the animator that manages entire scene transitions.")]
        [SerializeField]
        protected Animator _sceneTransitionAnimator;
        /// <summary>
        /// All available menus.
        /// </summary>
        protected Dictionary<MenuType, MenuTransition> _availableMenus = new Dictionary<MenuType, MenuTransition>();
        /// <summary>
        /// Stores the most recent transition.
        /// </summary>
        protected MenuTransitionStepHistory _menuTransitionsHistory;
        /// <summary>
        /// Should the most recent transition be remembered so it can be optionally reversed later?
        /// </summary>
        protected bool _rememberStep = true;
        /// <summary>
        /// Currently active menu.
        /// </summary>
        protected MenuType _currentMenu = MenuType.None;
        protected void Start()
        {
            //don't have to be found. For debug purposes for example.
            if (_menusParent != null)
            {
                var foundMenus = _menusParent.GetComponentsInChildren<MenuTransition>();
                if (foundMenus != null)
                {
                    foreach (var menu in foundMenus)
                    {
                        _availableMenus.Add(menu.MenuType, menu);
                    }
                }
            }

            if(_startingMenu != null)
            {
                PerformTransition(_startingMenu.GetStartMenu());
            }
        }
        /// <summary>
        /// Performs transition from given menu to given menu, in main menu.
        /// </summary>
        /// <param name="fromMenu"></param>
        /// <param name="toMenu"></param>
        public void PerformTransition(MenuType toMenu)
        {
            //Get and change visibility of the current and next menu.
            var currentMenu = GetVisibilityControllerForMenu(_currentMenu);
            currentMenu?.HideMenu();

            var nextMenu = GetVisibilityControllerForMenu(toMenu);
            nextMenu?.ShowMenu();

            if (_rememberStep)
            {
                _menuTransitionsHistory = new MenuTransitionStepHistory()
                {
                    TransitionFrom = _currentMenu,
                    TransitionTo = toMenu
                };
            }
            
            ReportNewControls(nextMenu);
            //Fade out current menu.
            StartCoroutine(FadeOut(currentMenu));
            
            //Fade in the next menu.
            StartCoroutine(FadeIn(nextMenu));

            _currentMenu = toMenu;
            if (toMenu == MenuType.None)
            {
                _onInGameMenuTurnedOff?.Invoke();
            }
            else if (toMenu == MenuType.Exit)
            {
                LeaveGame();
            }
        }
        /// <summary>
        /// Performs the fade out scene transition.
        /// </summary>
        public void PerformSceneTransition(string scenePath)
        {
            //Create data during fadeout.
            StartCoroutine(CoordinateSceneTransition(scenePath));

        }
        /// <summary>
        /// Forces return to previous menu. Does not use history but uses the backwards transitions serialized object to determine
        /// which menu shall the game return to.
        /// </summary>
        public virtual void PerformReturnTransition()
        {
            var targetMenu = _backwardsTransitions.GetMainMenuBackwardsTransition(_currentMenu);
            PerformTransition(targetMenu);
        }
        /// <summary>
        /// Causes the manager to revert the last made main menu transition. Reverse transition will not be remembered.
        /// </summary>
        public void RevertLastTransition()
        {
            //Reverse the last made transition but do not remember doing this.
            _rememberStep = false;
            PerformTransition( _menuTransitionsHistory.TransitionFrom);
            _rememberStep = true;
        }
        /// <summary>
        /// Called to leave the application.
        /// </summary>
        public void LeaveGame()
        {
            ApplicationQuitter.Instance.Quit();
        }
        /// <summary>
        /// Coordinates transition between scenes, starting and awaiting for coroutines.
        /// </summary>
        /// <param name="scenePath"></param>
        /// <returns></returns>
        private IEnumerator CoordinateSceneTransition(string scenePath)
        {
            var sceneFadeOutCoroutine = StartCoroutine(SceneFadeOut());
            var dataIniCoroutine = StartCoroutine(_sceneDataInitializer.CreateData());

            Debug.Break();

            yield return dataIniCoroutine;
            yield return sceneFadeOutCoroutine;

            SceneManager.LoadScene(scenePath);
        }
        /// <summary>
        /// Sends info about found controls for new menu to all listening objects. If any controls were found, that is.
        /// </summary>
        /// <param name="newMenu"></param>
        protected void ReportNewControls(MenuTransition newMenu)
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
        protected MenuTransition GetVisibilityControllerForMenu(MenuType menuType)
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
        protected IEnumerator FadeIn(MenuTransition transition)
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
        protected IEnumerator FadeOut(MenuTransition transition)
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
        protected IEnumerator SceneFadeOut()
        {
            _sceneTransitionAnimator.SetTrigger(_sceneFadeOutTrigger);

            //Wait for _sceneFadeOutTime in milliseconds.
            yield return new WaitForSecondsRealtime(_sceneFadeOutTime);
        }
        /// <summary>
        /// Performs the scene fade out.
        /// </summary>
        protected IEnumerator SceneFadeOut(SceneId sceneId)
        {
            _sceneTransitionAnimator.SetTrigger(_sceneFadeOutTrigger);

            yield return new WaitForSeconds(_sceneFadeOutTime);

            SceneManager.LoadScene((int)sceneId);
        }
    }

    
}