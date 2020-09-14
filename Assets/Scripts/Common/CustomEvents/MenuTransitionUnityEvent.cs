
using System;
using Assets.Scripts.Common.Enums;
using UnityEngine.Events;

namespace Assets.Scripts.Common.CustomEvents
{
    /// <summary>
    /// Custom unity event type that accepts two integers as arguments.
    /// </summary>
    [Serializable]
    public class MenuTransitionUnityEvent : UnityEvent<MenuType, MenuType>
    {
    }
}
