using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Common.Enums
{
    public enum KeyStateEnum
    {
        /// <summary>
        /// The user started pressing the button.
        /// </summary>
        Down = 0,
        /// <summary>
        /// The user is continuously holding the button pressed.
        /// </summary>
        Held = 1,
        /// <summary>
        /// The user stopped pressing the button.
        /// </summary>
        Up = 2

    }
}
