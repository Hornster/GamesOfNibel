using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Common.Enums
{
    /// <summary>
    /// Allows for recognition which canvas is which during map loading.
    /// </summary>
    public enum InGameUIType
    {
        /// <summary>
        /// The topmost in game menu, taking entire screen in screen division mode.
        /// </summary>
        TopUI,
        /// <summary>
        /// Part of UI that concerns only one player in screen division mode.
        /// </summary>
        PlayerUI
    }
}
