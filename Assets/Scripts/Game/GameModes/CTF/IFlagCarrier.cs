using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.GameModes.CTF.Entities;
using UnityEngine;

namespace Assets.Scripts.Game.GameModes.CTF
{
    /// <summary>
    ///Defines what should the flag carrier be able to do.
    /// </summary>
    public interface IFlagCarrier
    {
        /// <summary>
        /// Gets the transform of the flag carrier.
        /// </summary>
        Transform MyTransform { get; }
        /// <summary>
        /// Gets the transform of the flag position while carrying.
        /// </summary>
        Transform FlagPosition { get; }
        /// <summary>
        /// Which team does this carrier belong to?
        /// </summary>
        Teams MyTeam { get; }
        /// <summary>
        /// Is this carrier carrying at least one flag?
        /// </summary>
        bool HasFlag { get; }
        /// <summary>
        /// Makes the carrier give the flag to the caller.
        /// </summary>
        /// <param name="takerHash">The unique hash of the object that took the flag.</param>
        /// <returns></returns>
        IFlag TakeOverFlag(int takerHash);
        /// <summary>
        /// Called when the carrier picks up a flag from the ground/air.
        /// </summary>
        /// <param name="flag"></param>
        void PickedUpFlag(IFlag flag);
    }
}
