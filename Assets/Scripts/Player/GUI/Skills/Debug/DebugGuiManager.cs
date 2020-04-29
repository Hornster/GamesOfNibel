using System.Collections.Generic;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Player.GUI.Skills.Debug
{
    public class DebugGuiManager : MonoBehaviour
    {
        [SerializeField]
        private ISkillDebugInfo _doubleJumpDbgImg;
        [SerializeField]
        private ISkillDebugInfo _wallSlideDbgImg;
        [SerializeField]
        private ISkillDebugInfo _wallJumpDbgImg;
        [SerializeField]
        private ISkillDebugInfo _glideDbgImg;
        [SerializeField]
        private ISkillDebugInfo _bashDbgImg;
        [SerializeField]
        private ISkillDebugInfo _dashSlideDbgImg;
        [SerializeField]
        private ISkillDebugInfo _grappleSlideDbgImg;

        private Dictionary<SkillType, ISkillDebugInfo> _skillDbgInfo;

        private void Start()
        {
            _skillDbgInfo = new Dictionary<SkillType, ISkillDebugInfo>();
            _skillDbgInfo.Add(SkillType.DoubleJump, _doubleJumpDbgImg);
            _skillDbgInfo.Add(SkillType.WallSlide, _wallSlideDbgImg);
            _skillDbgInfo.Add(SkillType.WallJump, _wallJumpDbgImg);
        }

        public void UsedSkill(SkillType skillType)
        {
            if (_skillDbgInfo.TryGetValue(skillType, out var usedSkill))
            {
                usedSkill.SkillWasUsed();
            }
        }

        public void SkillWasReset(SkillType skillType)
        {
            if (_skillDbgInfo.TryGetValue(skillType, out var resetSkill))
            {
                resetSkill.SkillWasReset();
            }
        }
    }
}