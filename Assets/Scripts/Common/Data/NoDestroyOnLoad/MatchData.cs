using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Exceptions;
using UnityEngine;

namespace Assets.Scripts.Common.Data.NoDestroyOnLoad
{
    /// <summary>
    /// Stores data of all players that will be partaking in the match and need to be spawned.
    /// </summary>
    public class MatchData : MonoBehaviour
    {
        /// <summary>
        /// The instance of the match data.
        /// </summary>
        private static MatchData _instance;
        /// <summary>
        /// How many instances are active? Only one is allowed.
        /// </summary>
        private static int _instancesCount = 0;
        [Tooltip("The parent gameobject that should be marked as DontDestroyOnLoad.")]
        [SerializeField]
        private GameObject _parent;
        /// <summary>
        /// Configuration of skills that will be used during the match.
        /// </summary>
        [Tooltip("Configuration of skills for the match.")]
        [SerializeField]
        private SkillsConfig _skillsConfig;

        /// <summary>
        /// Stores data of all players that needs to be spawned.
        /// </summary>
        public List<PlayerConfig> PlayersConfigs { get; private set; } = new List<PlayerConfig>();
        /// <summary>
        /// Gets the skills configuration.
        /// </summary>
        public SkillsConfig SkillsConfig => _skillsConfig;


        private void Awake()
        {
            if (_instancesCount > 0)
            {
                throw new Exception($"There are multiple instances of {this} this singleton!");
            }
            _instancesCount++;
            _instance = this;
            DontDestroyOnLoad(_parent);
        }
        /// <summary>
        /// Adds given config to the list.
        /// </summary>
        /// <param name="config">Player config.</param>
        public void AddPlayerConfig(PlayerConfig config)
        {
            PlayersConfigs.Add(config);
        }
        /// <summary>
        /// Removes all saved player configurations.
        /// </summary>
        public void ClearPlayerConfigs()
        {
            PlayersConfigs.Clear();
        }
        /// <summary>
        /// Get instance of the match data.
        /// </summary>
        /// <returns></returns>
        public static MatchData GetInstance()
        {
            if (_instance == null)
            {
                throw new GONBaseException("No MatchData instance is present while trying to retrieve it!");
            }
            return _instance;
        }

        public void OnDestroy()
        {
            _instance = null;
        }
    }
}
