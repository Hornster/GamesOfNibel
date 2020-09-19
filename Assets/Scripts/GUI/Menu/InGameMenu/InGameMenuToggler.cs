using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.CustomEvents;
using Assets.Scripts.Common.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GUI.Menu.InGameMenu
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
        private MenuTransitionUnityEvent _menuTransitionEvent;
        [Tooltip("Which menu is this script toggling.")]
        [SerializeField]
        private MenuType _toggledMenu = MenuType.PauseMenu;
        /// <summary>
        /// Set to true when in game menu is active.
        /// </summary>
        private bool _isIngameMenuActive;

        /// <summary>
        /// Activates the game menu by sending call to menu transition manager.
        /// </summary>
        public void EnableInGameMenu()
        {
            _isIngameMenuActive = true;
            _menuTransitionEvent?.Invoke(_toggledMenu);
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
    }
}
//TODO - add this as gameobject
//TODO - make the input reader from player register toggling value and method here
//TODO - register to menu transition manager for getting information when menu gets closed
//TODO - make menu transition manager register here to  know when the menu needs to be launched