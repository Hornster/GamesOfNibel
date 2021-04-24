using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.GUI.Menu.MapSelection
{
    /// <summary>
    /// Stores state of the skills selected for the match.
    /// </summary>
    public class SkillsState : MonoBehaviour
    {
        [Tooltip("Used to transfer data from and to between-scene-data-carrying object.")]
        [SerializeField]
        private MatchDataHook _matchDataHook;

        /// <summary>
        /// Stores handlers for skill states config restore for the UI upon returning to menu.
        /// </summary>
        private Dictionary<SkillType, UnityAction<bool>> _skillDataToUIEvents = new Dictionary<SkillType, UnityAction<bool>>();
        /// <summary>
        /// Stores current states of all available skills.
        /// </summary>
        private Dictionary<SkillType, bool> _skillsStates = new Dictionary<SkillType, bool>();

        private void Start()
        {
            var skillControlManagers = GetComponentsInChildren<SkillControlManager>().ToList();
            ConnectSkillUIControls(skillControlManagers);

            var allSkills = Enum.GetValues(typeof(SkillType)) as SkillType[];
            foreach (var skillType in allSkills)
            {
                _skillsStates.Add(skillType, true);//By default all skills are turned on.
            }

            var skillsData = MatchDataHook.MatchDataReference;
            if (skillsData?.SkillsConfig != null)
            {
                SetSkillState(skillsData.SkillsConfig);
                //TODO call SetSkillsState, restore config of skills (when such operation is possible)
            }
        }
        /// <summary>
        /// Sets the state of the skill. Used by UI elements.
        /// </summary>
        /// <param name="skill">What skill to set the state of.</param>
        /// <param name="skillState">True for enabled skill, false for disabled one.</param>
        public void SetSkillState(SkillType skill, bool skillState)
        {
            if (_skillsStates.ContainsKey(skill))
            {
                _skillsStates[skill] = skillState;
                MatchDataHook.MatchDataReference.SkillsConfig.SetAvailableSkill(skill, skillState);
                //TODO inform the MatchData about the change. Call it via MatchDataHook static ref.
            }
        }

        /// <summary>
        /// Sets the state of the skill. Used to restore data in UI elements after loading the menu.
        /// </summary>
        /// <param name="skillsConfig">Skills configuration object.</param>
        private void SetSkillState(SkillsConfig skillsConfig)
        {
            var availableSkills = skillsConfig.AvailableSkills.dictionary;
            var allSkills = Enum.GetValues(typeof(SkillType)) as SkillType[];

            for (int i = 0; i < allSkills.Length; i++)
            {
                var currentSkill = allSkills[i];
                _skillDataToUIEvents[currentSkill].Invoke(availableSkills[currentSkill]);
                //TODO when the array in skillconfig wil become a dictionary, read it and use it to
                //TODO inform GUI elements about last state of skills selected by the player.
            }
        }
        /// <summary>
        /// Connects the control managers of the skills with this skill state container.
        /// </summary>
        /// <param name="skillControlManagers"></param>
        private void ConnectSkillUIControls(List<SkillControlManager> skillControlManagers)
        {
            foreach (var skillControlManager in skillControlManagers)
            {
                //Connect the SetSkillState method of this class with skill UI elements so these can update
                //the state of the skills when player interacts with them.
                skillControlManager.RegisterOnSkillSelectionChange(this.SetSkillState);
                //Also, let the skill control managers register their own handlers. When the player leaves
                //the game and gets back to the menu, these handlers will be used to restore the config.
                _skillDataToUIEvents.Add(skillControlManager.SkillType, skillControlManager.SetSkillState);
            }
        }
    }
}
