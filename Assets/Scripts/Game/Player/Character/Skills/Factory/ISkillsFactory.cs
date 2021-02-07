using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Player.Character.Skills.Factory
{
    public interface ISkillsFactory
    {
        /// <summary>
        /// Creates a new skill of provided type, assigning it as child to provided parent.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="playerState"></param>
        /// <param name="rb"></param>
        /// <param name="skillType"></param>
        /// <returns></returns>
        IBasicSkill CreateSkill(Transform parent, PlayerState playerState, Rigidbody2D rb, SkillType skillType);
    }
}
