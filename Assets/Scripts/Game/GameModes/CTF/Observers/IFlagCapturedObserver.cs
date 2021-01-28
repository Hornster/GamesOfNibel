using Assets.Scripts.Game.Common.Enums;
using UnityEngine.Events;

namespace Assets.Scripts.Game.GameModes.CTF.Observers
{
    /// <summary>
    /// Defines setting for an observed object that concerns flag capturing.
    /// </summary>
    public interface IFlagCapturedObserver
    {
        /// <summary>
        /// Register a handler for the observed object.
        /// </summary>
        /// <param name="handler">Handler with arguments:
        /// Teams - the team that captured the flag.
        /// int - amount of captured flags.</param>
        void RegisterObserver(UnityAction<Teams, int> handler);
    }
}
