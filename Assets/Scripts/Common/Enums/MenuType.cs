using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.Enums
{
    public enum MenuType
    {
        /// <summary>
        /// Menu seen as the first upon launching the game.
        /// </summary>
        WelcomeMenu,
        /// <summary>
        /// Self explanatory.
        /// </summary>
        MapSelectionMenu,
        /// <summary>
        /// Can be used for example when there's a transition from a menu to a playable scene and backwards.
        /// </summary>
        None
    }
}
