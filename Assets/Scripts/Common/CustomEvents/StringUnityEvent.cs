using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Assets.Scripts.Common.CustomEvents
{
    /// <summary>
    /// Unity event that accepts single string as an argument.
    /// </summary>
    [Serializable]
    public class StringUnityEvent : UnityEvent<string>
    {
    }
}
