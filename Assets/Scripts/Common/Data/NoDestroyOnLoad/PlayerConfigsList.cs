using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//Use GetInstanceID for the player ids.

namespace Assets.Scripts.Common.Data.NoDestroyOnLoad
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
