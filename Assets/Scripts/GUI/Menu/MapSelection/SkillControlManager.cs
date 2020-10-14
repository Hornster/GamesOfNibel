using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.CustomEvents;
using Assets.Scripts.Common.Enums;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.Menu.MapSelection
{
    /// <summary>
    /// Manages the look of the skill toggling controls.
    /// </summary>
    public class SkillControlManager : MonoBehaviour
    {
        [Tooltip("Called whenever the connected skill is toggled.")]
        private UnityAction<SkillType, bool> _onSkillSelectionChange;
        /// <summary>
        /// Provides infor about available colors for the skills in the menu.
        /// </summary>
        [SerializeField] private SkillMenuColors _skillMenuColors;
        [SerializeField] private Image _skillIcon;
        /// <summary>
        /// The type of the skill this toggle toggles.
        /// </summary>
        [SerializeField] private SkillType _skillType;
        /// <summary>
        /// Is the skill required by currently previewed map?
        /// </summary>
        private bool isSkillRequired;
        /// <summary>
        /// Which skill is this control responsible toggling for.
        /// </summary>
        public SkillType SkillType => _skillType;

        public bool IsSkillActive { get; private set; } = true;
        /// <summary>
        /// Is this skill required for currently PREVIEWED map? Causes automatic update of skill color.
        /// </summary>
        /// <param name="isRequired"></param>
        public void UpdateSkillRequirementState(bool isRequired)
        {
            isSkillRequired = isRequired;
            UpdateSkillColor();
        }
        /// <summary>
        /// Sets the skill to provided state.
        /// </summary>
        /// <param name="isActive">True for enabled, False for disabled.</param>
        public void SetSkillState(bool isActive)
        {
            IsSkillActive = isActive;
            UpdateSkillColor();
        }
        /// <summary>
        /// Toggles the skill. Automatically calls color update.
        /// </summary>
        public void ToggleSkill()
        {
            IsSkillActive = !IsSkillActive;
            _onSkillSelectionChange?.Invoke(_skillType, IsSkillActive);
            UpdateSkillColor();
        }
        /// <summary>
        /// Registers provided handler. When the user interacts with UI control assigned to this skill,
        /// the handler will be called.
        /// </summary>
        /// <param name="handler"></param>
        public void RegisterOnSkillSelectionChange(UnityAction<SkillType, bool> handler)
        {
            _onSkillSelectionChange += handler;
        }
        /// <summary>
        /// Updates the color of the switch accordingly to its current state.
        /// </summary>
        private void UpdateSkillColor()
        {
            if (isSkillRequired)
            {
                if (IsSkillActive)
                {
                    _skillIcon.color = _skillMenuColors.EnabledSkillNeeded;
                }
                else
                {
                    _skillIcon.color = _skillMenuColors.DisabledSkillNeeded;
                }
            }
            else
            {
                if (IsSkillActive)
                {
                    _skillIcon.color = _skillMenuColors.EnabledSkill;
                }
                else
                {
                    _skillIcon.color = _skillMenuColors.DisabledSkill;
                }
            }
        }
        private void Start()
        {
            UpdateSkillColor();
        }

        private void OnDestroy()
        {
            _onSkillSelectionChange = null;
        }
    }
}
