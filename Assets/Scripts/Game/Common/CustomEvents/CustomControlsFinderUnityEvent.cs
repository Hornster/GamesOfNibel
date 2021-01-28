using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.GUI.Menu;
using UnityEngine.Events;

namespace Assets.Scripts.Common.CustomEvents
{
    /// <summary>
    /// Unity event that can be shown in the inspector. Allows for passing CustomControlsFinder
    /// types as arguments.
    /// </summary>
    [Serializable]
    public class CustomControlsFinderUnityEvent : UnityEvent<CustomControlsFinder>
    {
    }
}
