namespace Assets.Scripts.Game.Common.Enums
{
    public enum MenuType
    {
        /// <summary>
        /// Menu seen as the first upon launching the game.
        /// </summary>
        WelcomeMenu,
        /// <summary>
        /// Self explanatory.
        /// </summary>
        MapSelectionMenu,
        /// <summary>
        /// Parent menu for various settings tabs.
        /// </summary>
        SettingsMenu,
        /// <summary>
        /// Menu available only during a match.
        /// </summary>
        PauseMenu,
        /// <summary>
        /// Can be used for example when there's a transition from a menu to a playable scene and backwards.
        /// </summary>
        None
    }
}
