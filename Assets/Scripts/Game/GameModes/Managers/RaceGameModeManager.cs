using Assets.Scripts.Game.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.GameModes.Managers
{
    [RequireComponent(typeof(Timer))]
    public class RaceGameModeManager : MonoBehaviour
    {
        //private 

        //TODO Make player controller have an ID assigned through factory.
        //TODO Factory would receive the id from data object
        //TODO This class would  have refs to all players' GUIs and boolean indicating if they finished the run.
        //TODO Or players would have special component for racing, idk.
        //TODO The refs and boolean would be in dict, ID would be the key.
        //TODO upon running into the finish line the ID would be sent
        //TODO this class would pass the victory info and stop the timers.
        //TODO In Update, whoever has not finished the race yet would have their UI updated every frame.
        //TODO You could use timer with TimeOffset (or something like that) structure from .Net. It could be initialized with
        //TODO seconds, then read hours, minutes and seconds part for easy formatting.
    }
}
