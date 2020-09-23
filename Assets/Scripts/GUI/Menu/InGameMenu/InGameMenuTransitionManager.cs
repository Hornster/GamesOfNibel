using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.GUI.Menu.Transitions;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GUI.Menu.InGameMenu
{
    /// <summary>
    /// In-Game version of transition manager. Used for menus that can be show during matches.
    /// </summary>
    public class InGameMenuTransitionManager : MenuTransitionManager
    {
        public override void PerformReturnTransition()
        {
            var targetMenu = _backwardsTransitions.GetIngameMenuBackwardsTransition(_currentMenu);
            PerformTransition(targetMenu);
        }
        /// <summary>
        /// Performs transition to main menu.
        /// </summary>
        public void PerformReturnToMenu()
        {
            StartCoroutine(SceneFadeOut(_startingMenu.MenuSceneId));
        }
    }
}
