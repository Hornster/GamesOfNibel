using System;
using Assets.Scripts.Game.GUI.Menu;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Common.CustomEvents
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
