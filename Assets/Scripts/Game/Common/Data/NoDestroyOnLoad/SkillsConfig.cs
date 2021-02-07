using System;
using Assets.Scripts.Game.Common.CustomCollections.DefaultCollectionsSerialization.Dictionary;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Data.NoDestroyOnLoad
{
    /// <summary>
    /// Configuration of the available skills for currently played map.
    /// </summary>
    public class SkillsConfig : MonoBehaviour
    {
        /// <summary>
        /// What skills are allowed for the player to use.
        /// </summary>
        [Tooltip("What skills are allowed for the player to use.")]
        [SerializeField]
        private SkillTypeBoolDictionary _availableSkills;


        private void Awake()
        {
            var allSkills = Enum.GetValues(typeof(SkillType)) as SkillType[];
            foreach (var skill in allSkills)
            {
#if UNITY_EDITOR
                if (_availableSkills.dictionary.ContainsKey(skill))
                {
                    //If modifications will be introduced via unity inspector, do not overwrite them.
                    continue;
                }
#endif
                _availableSkills.dictionary.Add(skill, true);
            }
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Getters for the code.
        /// </summary>
        public SkillTypeBoolDictionary AvailableSkills
        {
            get => _availableSkills;
            set => _availableSkills = value;
        }
        /// <summary>
        /// Adds or removes, accordingly to passed parameters, given skill from configuration.
        /// </summary>
        /// <param name="skillType">What skill to set.</param>
        /// <param name="isActive">If true - adds the skill. If false - removes it.</param>
        public void SetAvailableSkill(SkillType skillType, bool isActive)
        {
            _availableSkills.dictionary[skillType] = isActive;
        }
    }
}
