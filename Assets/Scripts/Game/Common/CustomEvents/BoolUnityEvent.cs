using System;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Common.CustomEvents
{
    /// <summary>
    /// Unity event with one bool argument.
    /// </summary>
    [Serializable]
    public class BoolUnityEvent : UnityEvent<bool>
    {
    }
}
