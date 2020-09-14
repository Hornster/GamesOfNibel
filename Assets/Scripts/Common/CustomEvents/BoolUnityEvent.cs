using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Assets.Scripts.Common.CustomEvents
{
    /// <summary>
    /// Unity event with one bool argument.
    /// </summary>
    [Serializable]
    public class BoolUnityEvent : UnityEvent<bool>
    {
    }
}
