﻿using Assets.Scripts.Game.Common.Enums;

namespace Assets.Scripts.Game.GUI.Gamemodes.CTF
{
    /// <summary>
    /// GUI for the CTF information parts.
    /// </summary>
    public interface IGUIInterface
    {
        int OwningPlayerID { get; set; }
        /// <summary>
        /// Shows informational message.
        /// </summary>
        /// <param name="whichTeam">Which team does the message concern.</param>
        /// <param name="message">What exactly should be shown.</param>
        void PrintMessage(Teams whichTeam, string message);
        /// <summary>
        /// Used to show the state of calling the unstuck option by the player.
        /// </summary>
        /// <param name="keyState"></param>
        void ShowUnstuck(KeyStateEnum keyState);
    }
}
