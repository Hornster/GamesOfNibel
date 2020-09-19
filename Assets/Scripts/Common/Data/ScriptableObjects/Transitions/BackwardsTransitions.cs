using System.Collections.Generic;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Exceptions;
using UnityEngine;

namespace Assets.Scripts.Common.Data.ScriptableObjects.Transitions
{
    /// <summary>
    /// Stores information about backwards transitions for menu and ingame menu.
    /// </summary>
    [CreateAssetMenu(fileName = "BackwardsTransitions", menuName = "ScriptableObjects/Transitions/BackwardsTransitions")]
    public class BackwardsTransitions : ScriptableObject
    {
        /// <summary>
        /// Key is the current menu, value is the menu the game shall return to. Dictionary for main menu.
        /// </summary>
        private Dictionary<MenuType, MenuType> _menuBackwardsTransitions = new Dictionary<MenuType, MenuType>();
        /// <summary>
        /// Key is the current menu, value is the menu the game shall return to. Dictionary for in-game menu.
        /// </summary>
        private Dictionary<MenuType, MenuType> _inGameMenuBackwardsTransitions = new Dictionary<MenuType, MenuType>();

        private void Awake()
        {
            AddTransitionsForMainMenu();
            AddTransitionsForIngameMenu();
        }
        /// <summary>
        /// Declares all backwards transitions for main menu.
        /// </summary>
        private void AddTransitionsForMainMenu()
        {
            _menuBackwardsTransitions.Add(MenuType.WelcomeMenu, MenuType.WelcomeMenu);
            _menuBackwardsTransitions.Add(MenuType.MapSelectionMenu, MenuType.WelcomeMenu);
        }
        /// <summary>
        /// Declares all backwards transitions for ingame menu.
        /// </summary>
        private void AddTransitionsForIngameMenu()
        {
            _inGameMenuBackwardsTransitions.Add(MenuType.PauseMenu, MenuType.None);
            _inGameMenuBackwardsTransitions.Add(MenuType.SettingsMenu, MenuType.PauseMenu);
        }
        /// <summary>
        /// Gets the target menu for backwards transition from provided menu. If no equivalent found - throws exception of
        /// DictionaryEntryNotFoundException. 
        /// </summary>
        /// <param name="currentMenuType"></param>
        /// <returns></returns>
        public MenuType GetMainMenuBackwardsTransition(MenuType currentMenuType)
        {
            if (_menuBackwardsTransitions.TryGetValue(currentMenuType, out var targetMenuType))
            {
                return targetMenuType;
            }

            throw new DictionaryEntryNotFoundException($"There is no return menu assigned to {currentMenuType} menu!");
        }
        /// <summary>
        /// Gets the target menu for backwards transition from provided menu. If no equivalent found - throws exception of 
        /// DictionaryEntryNotFoundException. 
        /// </summary>
        /// <param name="currentMenuType"></param>
        /// <returns></returns>
        public MenuType GetIngameMenuBackwardsTransition(MenuType currentMenuType)
        {
            if (_inGameMenuBackwardsTransitions.TryGetValue(currentMenuType, out var targetMenuType))
            {
                return targetMenuType;
            }

            throw new DictionaryEntryNotFoundException($"There is no return in-game menu assigned to {currentMenuType} menu!");
        }
    }
}
