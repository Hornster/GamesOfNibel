using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Player.Character.Skills
{
    public class SkillsController : MonoBehaviour
    {
        /// <summary>
        /// Reference to the gameobject that stores skills of the character.
        /// </summary>
        [SerializeField] private GameObject _basicSkillContainer;
        /// <summary>
        /// Stores basic skills that are available for the player.
        /// </summary>
        private Dictionary<SkillType, IBasicSkill> _basicSkills;
        /// <summary>
        /// Adds the skill to the skill set of the character. If skill was added - returns true.
        /// Returns false if skill was already present.
        /// </summary>
        /// <param name="type">Type of the skill by which the skill is recognized and selected.</param>
        /// <param name="skill">The skill object itself.</param>
        public bool AddBasicSkill(SkillType type, IBasicSkill skill)
        {
            if (_basicSkills.ContainsKey(type))
            {
                _basicSkills.Add(type, skill);
                return true;
            }

            return false;
        }
        /// <summary>
        /// Makes the character use skill of provided type. If the skill was given to the character,
        /// it will be used.
        /// </summary>
        /// <param name="type">Type of the skill to use.</param>
        public void UseSkill(SkillType type)
        {
            IBasicSkill usedSkill;
            if (_basicSkills.TryGetValue(type, out usedSkill))
            {
                usedSkill.UseSkill();
            }
        }
        /// <summary>
        /// Retrieves the gameobject that holds all base skills of the character.
        /// </summary>
        /// <returns></returns>
        public GameObject GetSkillContainer()
        {
            return _basicSkillContainer;
        }
    }
}
