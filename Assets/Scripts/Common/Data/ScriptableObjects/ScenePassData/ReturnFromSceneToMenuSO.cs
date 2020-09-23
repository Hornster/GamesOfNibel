using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Common.Data.ScriptableObjects.ScenePassData
{
    /// <summary>
    /// Holds data defining which menu shall the game return to after changing scenes.
    /// </summary>
    [CreateAssetMenu(fileName = "ReturnFromSceneTomMenuSO", menuName = "ScriptableObjects/SceneChanging/ReturnFromSceneToMenuSO", order = 1)]
    public class ReturnFromSceneToMenuSO : ScriptableObject
    {
        [Tooltip("What menu shall be shown during game startup.")]
        [SerializeField]
        private MenuType _startupMenu;
        [Tooltip("What menu shall be shown after leaving the match.")]
        [SerializeField]
        private MenuType _returnFromMatchToMenu;
        [Tooltip("The build order number of the main menu scene.")]
        [SerializeField]
        private SceneId _menuSceneId = SceneId.MainMenuScene;
        /// <summary>
        /// The build order number of the main menu scene.
        /// </summary>
        public SceneId MenuSceneId => _menuSceneId;
        /// <summary>
        /// Set to true after first load of the main menu.
        /// </summary>
        private static bool _gameLoaded = false;
        public MenuType GetStartMenu()
        {
            //When the game is loading, we should start from the welcome menu.
            //Later on we return to the main menu only from the match, so we should
            //load the menu and return to menu which we started the match from.
            if (_gameLoaded == false)
            {
                _gameLoaded = true;
                return _startupMenu;
            }

            return _returnFromMatchToMenu;
        }
    }
}
