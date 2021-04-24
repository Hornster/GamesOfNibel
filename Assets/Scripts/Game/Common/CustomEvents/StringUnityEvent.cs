using System;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Common.CustomEvents
{
    /// <summary>
    /// Unity event that accepts single string as an argument.
    /// </summary>
    [Serializable]
    public class StringUnityEvent : UnityEvent<string>
    {
    }
}
