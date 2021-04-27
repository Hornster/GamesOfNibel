using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Data.Maps;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Common.CustomEvents
{
    /// <summary>
    /// Used to transport map data without strong coupling.
    /// </summary>
    [Serializable]
    public class MapDataUnityEvent : UnityEvent<MapData>
    {
    }
}
