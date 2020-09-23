using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.Enums
{
    /// <summary>
    /// Scene build order IDs.
    /// </summary>
    public enum SceneId
    {
        //Only the scenes that are in the game itself, NOT LOADED AS MODS, can go in here.
        //Keep it in sync with the build order!
        MainMenuScene = 0
    }
}
