using Assets.Scripts.Game.GUI.Menu.Transitions;

namespace Assets.Scripts.Game.GUI.Menu.InGameMenu
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
