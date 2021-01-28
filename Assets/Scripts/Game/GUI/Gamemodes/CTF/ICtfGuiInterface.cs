using Assets.Scripts.Game.Common.Enums;

namespace Assets.Scripts.Game.GUI.Gamemodes.CTF
{
    /// <summary>
    /// GUI for the CTF information parts.
    /// </summary>
    public interface ICtfGuiInterface
    {
        /// <summary>
        /// Shows informational message.
        /// </summary>
        /// <param name="whichTeam">Which team does the message concern.</param>
        /// <param name="message">What exactly should be shown.</param>
        void PrintMessage(Teams whichTeam, string message);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whichTeam">Which team does the point change concern.</param>
        /// <param name="value">By how many points should the point amount be updated.</param>
        void UpdatePoints(Teams whichTeam, int value);
    }
}
