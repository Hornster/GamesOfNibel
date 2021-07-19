using Assets.Scripts.Game.Common.Enums;

namespace Assets.Scripts.Game.GUI.Gamemodes.CTF
{
    /// <summary>
    /// GUI for the CTF information parts.
    /// </summary>
    public interface IGUIInterface
    {
        /// <summary>
        /// Shows informational message.
        /// </summary>
        /// <param name="whichTeam">Which team does the message concern.</param>
        /// <param name="message">What exactly should be shown.</param>
        void PrintMessage(Teams whichTeam, string message);
    }
}
