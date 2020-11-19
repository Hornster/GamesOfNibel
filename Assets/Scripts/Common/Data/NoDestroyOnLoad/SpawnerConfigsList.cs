using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Common.Data.NoDestroyOnLoad
{
    public class SpawnerConfigsList : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Player menu config prefab.")]
        private GameObject _spawnerConfigPrefab;
        public List<SpawnerGroupConfig> SpawnerConfigs { get; } = new List<SpawnerGroupConfig>();

        /// <summary>
        /// Adds a spawn group config.
        /// </summary>
        /// <param name="spawnGroupTeam">What team do the spawns belong to.</param>
        /// <param name="quantity">How many spawns are in there.</param>
        public void AddSpawnConfig(Teams spawnGroupTeam, int quantity)
        {
            if (quantity > 1)
            {
                var newSpawnerConfig = Instantiate(_spawnerConfigPrefab).GetComponentInChildren<SpawnerGroupConfig>();
                newSpawnerConfig.MyTeam = spawnGroupTeam;
                newSpawnerConfig.Quantity = quantity;
                SpawnerConfigs.Add(newSpawnerConfig);
            }
        }
    }
}
