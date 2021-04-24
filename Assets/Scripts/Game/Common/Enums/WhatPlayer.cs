using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Common.Enums
{
    /// <summary>
    /// Marks players for split screen mode.
    /// </summary>
    public enum WhatPlayer
    {
        P1,
        P2,
        /// <summary>
        /// Client player is used in networking.
        /// </summary>
        Client
    }
}
