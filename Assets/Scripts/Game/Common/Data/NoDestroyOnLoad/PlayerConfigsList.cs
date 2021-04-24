using System.Collections.Generic;
using UnityEngine;

//Use GetInstanceID for the player ids.

namespace Assets.Scripts.Game.Common.Data.NoDestroyOnLoad
{
    /// <summary>
    /// Manages the configs of players.
    /// </summary>
    public class PlayerConfigsList : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Player menu config prefab.")]
        private GameObject _playerConfigPrefab;

        public List<PlayerConfig> PlayerConfigs { get; } = new List<PlayerConfig>();

        private void Awake()
        {
            var foundPlayerConfigs = GetComponentsInChildren<PlayerConfig>();
            foreach (var playerConfig in foundPlayerConfigs)
            {
                PlayerConfigs.Add(playerConfig);
            }
        }

        /// <summary>
        /// Creates new config for the player. Returns the id of the config.
        /// </summary>
        /// <returns></returns>
        public int AddPlayerConfig()
        {
            var newPlayerConfig = Instantiate(_playerConfigPrefab, this.transform).GetComponentInChildren<PlayerConfig>();
            newPlayerConfig.SetPlayerId(newPlayerConfig.GetInstanceID());
            PlayerConfigs.Add(newPlayerConfig);
            return newPlayerConfig.MyId;
        }
    }
}
