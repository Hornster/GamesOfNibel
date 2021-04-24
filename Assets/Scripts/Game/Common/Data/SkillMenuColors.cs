using UnityEngine;

namespace Assets.Scripts.Game.Common.Data
{
    /// <summary>
    /// Stores data about skills color selection.
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillMenuColors", order = 1)]
    public class SkillMenuColors : ScriptableObject
    {
        /// <summary>
        /// Enabled skill but not necessarily needed by the current map.
        /// </summary>
        public Color EnabledSkill;
        /// <summary>
        /// Disabled skill but not necessarily needed by the current map.
        /// </summary>
        public Color DisabledSkill;
        /// <summary>
        /// Enabled skill that is needed by the current map.
        /// </summary>
        public Color EnabledSkillNeeded;
        /// <summary>
        /// Disabled skill that is needed by the current map.
        /// </summary>
        public Color DisabledSkillNeeded;

    }
}
