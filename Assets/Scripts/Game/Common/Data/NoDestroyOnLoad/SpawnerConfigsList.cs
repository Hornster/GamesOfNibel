using System.Collections.Generic;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Data.NoDestroyOnLoad
{
    public class SpawnerConfigsList : MonoBehaviour
    {
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
        /// <param name="spawnerGroupConfig">Config of a spawner group that shall be added..</param>
        public void AddSpawnConfig(SpawnerGroupConfig spawnerGroupConfig)
        {
            spawnerGroupConfig.gameObject.transform.SetParent(this.transform);
            SpawnerConfigs.Add(spawnerGroupConfig);
        }

        public void ClearData()
        {
            foreach (var spawnerConfig in SpawnerConfigs)
            {
                spawnerConfig.DestroyConfig();
            }
            SpawnerConfigs.Clear();
        }
    }
}
