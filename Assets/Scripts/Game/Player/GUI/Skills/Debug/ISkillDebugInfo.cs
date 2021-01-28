using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Player.GUI.Skills.Debug
{
    /// <summary>
    /// Debug interface. Used to toggle GUI elements used to show given skill usage state.
    /// </summary>
    public interface ISkillDebugInfo
    {
        /// <summary>
        /// Called when a skill was used.
        /// </summary>
        void SkillWasUsed();
        /// <summary>
        /// Called when a skill was reset and can be used again.
        /// </summary>
        void SkillWasReset();
        /// <summary>
        /// Skill is recharged but cannot be currently used.
        /// </summary>
        void SkillBecameUnavailable();
        /// <summary>
        /// Skill is recharged and can be used right now.
        /// </summary>
        void SkillBecameAvailable();
    }
}
