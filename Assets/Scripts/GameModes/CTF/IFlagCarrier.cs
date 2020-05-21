using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.GameModes.CTF.Entities;
using UnityEngine;

namespace Assets.Scripts.GameModes.CTF
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
        /// <returns></returns>
        IFlag TakeOverFlag();
    }
}
