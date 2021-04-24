using System;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Common.CustomEvents
{
    /// <summary>
    /// Unity event with a SkillType and a bool arguments.
    /// </summary>
    [Serializable]
    public class SkillTypeBoolUnityEvent : UnityEvent<SkillType, bool>
    {
    }
}
