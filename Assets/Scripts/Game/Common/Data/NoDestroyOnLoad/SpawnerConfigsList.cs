using System.Collections.Generic;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Data.NoDestroyOnLoad
{
    public class SpawnerConfigsList : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Player menu config prefab.")]
        private GameObject _spawnerConfigPrefab;
        public List<SpawnerGroupConfig> SpawnerConfigs { get; } = new List<SpawnerGroupConfig>();

        private void Awake()
        {
            //Seek any already present configs. For example for debug purposes.
            var foundSpawnerConfigs = GetComponentsInChildren<SpawnerGroupConfig>();
            foreach (var spawnerGroupConfig in foundSpawnerConfigs)
            {
                SpawnerConfigs.Add(spawnerGroupConfig);
            }
        }
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
