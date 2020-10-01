using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Common.Data.NoDestroyOnLoad
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
        private SkillType[] _availableSkills;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
        
        /// <summary>
        /// Getters for the code.
        /// </summary>
        public SkillType[] AvailableSkills
        {
            get => _availableSkills;
            set => _availableSkills = value;
        }
    }
}
