﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;

namespace Assets.Scripts.GameModes.CTF.Entities
{
    /// <summary>
    /// Defines what should the flag be able to do.
    /// </summary>
    public interface IFlag
    {
        Teams MyTeam { get; }
        /// <summary>
        /// Is the flag carried by a player? Or any other thing that has the IFlagCarrier module?
        /// </summary>
        bool IsCarried { get; }
        /// <summary>
        /// Sets the new owner of this flag.
        /// </summary>
        /// <param name="newCarrier">The data of the new carrier of the flag.</param>
        void WasTakenOverBy(IFlagCarrier newCarrier);

        /// <summary>
        /// Called when the flag was successfully captured by a team.
        /// </summary>
        /// <param name="capturingEntity">The team that captured the flag.</param>
        void CaptureFlag(IFlagCarrier capturingEntity);
        /// <summary>
        /// Causes the carrier to drop carried flag.
        /// </summary>
        void DropCarriedFlag();
    }
}
