using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine.Events;

namespace Assets.Scripts.Common.CustomEvents
{
    /// <summary>
    /// Unity event with a SkillType and a bool arguments.
    /// </summary>
    [Serializable]
    public class SkillTypeBoolUnityEvent : UnityEvent<SkillType, bool>
    {
    }
}
