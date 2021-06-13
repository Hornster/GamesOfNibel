using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Common.CustomEvents
{
    [Serializable]
    public class TeamsUnityEvent : UnityEvent<Teams>
    {
    }
}
