using Assets.Scripts.Game.Common.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.GUI.Menu.InGameMenu
{
    /// <summary>
    /// Toggles the ingame menu and the input readers.
    /// </summary>
    public class InGameMenuToggler  : MonoBehaviour
    {
        /// <summary>
        /// Called when menu has been toggled. Passed argument is the state of the menu after toggling occured.
        /// </summary>
        private static UnityAction<bool> _onMenuToggle;

        /// <summary>
        /// Called when the ingame menu has to be shown.
        /// </summary>
        [Tooltip("Called when the ingame menu has to be shown.")]
        [SerializeField]
        private InGameMenuTransitionManager _transitionManager;
        [Tooltip("Which menu is this script toggling.")]
        [SerializeField]
        private MenuType _toggledMenu = MenuType.PauseMenu;
        /// <summary>
        /// Set to true when in game menu is active.
        /// </summary>
        private bool _isIngameMenuActive;

        private void Start()
        {
            GUIInputReader.RegisterOnCancelHandler(ToggleIngameMenu);
        }
        /// <summary>
        /// Toggles ingame menu. If it is enabled, causes the transition manager to return to previous menu.
        /// </summary>
        public void ToggleIngameMenu()
        {
            //Since the same button can activate the pause menu and cause return to previous menu
            if (_isIngameMenuActive == false)
            {
                //activate the menu if it is not active
                EnableInGameMenu();
                Debug.Log("Enabling menu.");
            }
        }
        /// <summary>
        /// Activates the game menu by sending call to menu transition manager.
        /// </summary>
        public void EnableInGameMenu()
        {
            _isIngameMenuActive = true;
            _transitionManager.PerformTransition(_toggledMenu);
            _onMenuToggle?.Invoke(_isIngameMenuActive);
        }
        /// <summary>
        /// Shall be called when in game menu (pause menu) has been disabled.
        /// </summary>
        public void OnInGameMenuDisabled()
        {
            _isIngameMenuActive = false;
            _onMenuToggle?.Invoke(_isIngameMenuActive);
        }
        /// <summary>
        /// Registers event handler that will be called whenever ingame menu is toggled. Passed argument is the state of the menu after toggling occured.
        /// </summary>
        /// <param name="handler"></param>
        public static void RegisterOnMenuToggledHandler(UnityAction<bool> handler)
        {
            _onMenuToggle += handler;
        }

        private void OnDestroy()
        {
            _onMenuToggle = null;
        }
    }
}
//TODO - add this as gameobject DONE
//TODO - make the input reader from player register toggling value and method here DONE
//TODO - register to menu transition manager for getting information when menu gets closed DONE
//TODO - make menu transition manager register here to  know when the menu needs to be launched DONE