using System;
using System.Collections.Generic;
using Assets.Scripts.Game.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Player.Character.Skills.Factory;
using Assets.Scripts.Game.Player.Data;
using UnityEngine;

namespace Assets.Scripts.Game.Player.Character.Skills
{
    public class SkillsController : MonoBehaviour
    {
        [Header("Required References")]
        /// <summary>
        /// Reference to the gameobject that stores skills of the character.
        /// </summary>
        [SerializeField] private GameObject _basicSkillContainer;
        /// <summary>
        /// Info about player state - what skills can they use, are they on the ground, etc.
        /// </summary>
        [SerializeField] private PlayerState _playerState;
        /// <summary>
        /// Stores basic skills that are available for the player.
        /// </summary>
        private Dictionary<SkillType, IBasicSkill> _basicSkills = new Dictionary<SkillType, IBasicSkill>();

        //DEBUG
        [Header("Debug")]
        [SerializeField] private DoubleJump _doubleJumpSkill;
        [SerializeField] private WallSlide _wallSlideSkill;
        [SerializeField] private WallJump _wallJumpSkill;
        [SerializeField] private Glide _glideSkill;

        private void Start()
        {
            AddSkillThroughDebug(SkillType.DoubleJump, _doubleJumpSkill);
            AddSkillThroughDebug(SkillType.WallSlide, _wallSlideSkill);
            AddSkillThroughDebug(SkillType.WallJump, _wallJumpSkill);
            AddSkillThroughDebug(SkillType.Glide, _glideSkill);
        }
        /// <summary>
        /// Adds skills that were added through inspector.
        /// </summary>
        /// <param name="skillType"></param>
        /// <param name="skill"></param>
        private void AddSkillThroughDebug(SkillType skillType, IBasicSkill skill)
        {
            if (skill != null)
            {
                AddBasicSkill(skillType, skill);
            }
        }
        //DEBUG


        
        private void Update()
        {
            //Perform reset of one-use skills if it is possible.
            _playerState.ChkSkillResetPossible();
        }
        /// <summary>
        /// Adds the skill to the skill set of the character. If skill was added - returns true.
        /// Returns false if skill was already present. If false returned, no new skill is added.
        /// </summary>
        /// <param name="type">Type of the skill by which the skill is recognized and selected.</param>
        /// <param name="skill">The skill object itself.</param>
        public bool AddBasicSkill(SkillType type, IBasicSkill skill)
        {
            if (_basicSkills.ContainsKey(type) == false)
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
