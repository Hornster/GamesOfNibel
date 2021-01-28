namespace Assets.Scripts.Game.GUI.Menu
{
    /// <summary>
    /// Stores possible colors for given skill.
    /// </summary>
    public enum MenuSkillColor
    {
        /// <summary>
        /// Color of enabled but NOT required skill for currently selected map.
        /// </summary>
        Enabled,
        /// <summary>
        /// Color of disabled but NOT required skill for currently selected map.
        /// </summary>
        Disabled,
        /// <summary>
        /// Color of enabled AND required skill for currently selected map.
        /// </summary>
        RequiredEnabled,
        /// <summary>
        /// Color of disabled AND required skill for currently selected map.
        /// </summary>
        RequiredDisabled
    }
}