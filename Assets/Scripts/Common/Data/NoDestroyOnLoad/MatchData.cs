using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// Configuration of skills that will be used during the match.
        /// </summary>
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
            _instance = this;
            DontDestroyOnLoad(this);
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
                throw new Exception("No MatchData instance is present while trying to retrieve it!");
            }
            return _instance;
        }

        public void OnDestroy()
        {
            _instance = null;
        }
    }
}
