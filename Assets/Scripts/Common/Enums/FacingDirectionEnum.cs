using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.Enums
{
    /// <summary>
    /// Defines directions that entities can face in the game.
    /// </summary>
    public enum FacingDirectionEnum
    {
        //Left shall be left as -1, right as 1. The values are used
        //for logical and math operations in the code.
        Left = -1,
        Right = 1
    }
}
