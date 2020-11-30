using System;
using System.Collections.Generic;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Common.Data.NoDestroyOnLoad
{
    /// <summary>
    /// Contains all necessary data for a scene to be loaded.
    /// </summary>
    public class SceneData : ISceneData 
    {
        [Tooltip("Used for visual organization only. All created players go here.")]
        [SerializeField]
        private Transform _playersParent;
        [Tooltip("Used for visual organization only. All created spawners go here.")]
        [SerializeField]
        private Transform _spawnersParent;
        /// <summary>
        /// List of all created players.
        /// </summary>
        public override List<GameObject> Players { get; protected set; } = new List<GameObject>();
        /// <summary>
        /// List of all created spawners.
        /// </summary>
        public override Dictionary<Teams, List<GameObject>> Spawners { get; protected set; } = new Dictionary<Teams, List<GameObject>>();

        /// <summary>
        /// Adds player to the data structure.
        /// </summary>
        /// <param name="playerObject">New player object.</param>
        public void AddPlayer(GameObject playerObject)
        {
            playerObject.transform.parent = _playersParent;
            Players.Add(playerObject);
        }

        /// <summary>
        /// Adds multiple spawners to the data structure.
        /// </summary>
        /// <param name="spawnerObjects">Spawners that should be added.</param>
        public void AddSpawners(List<GameObject> spawnerObjects)
        {
            foreach (var spawnerObject in spawnerObjects)
            {
                AddSpawner(spawnerObject);
            }
        }
        /// <summary>
        /// Adds single spawn to the data structure.
        /// </summary>
        /// <param name="spawnerObject">New spawner object.</param>
        public void AddSpawner(GameObject spawnerObject)
        {
            var team = spawnerObject.GetComponentInChildren<TeamModule>().MyTeam;
            spawnerObject.transform.parent = _spawnersParent;
            if (Spawners.TryGetValue(team, out var spawnersList))
            {
                spawnersList.Add(spawnerObject);
            }
            else
            {
                var newSpawnersList = new List<GameObject> { spawnerObject };
                Spawners.Add(team, newSpawnersList);
            }
        }
        /// <summary>
        /// Marks this object to be destroyed.
        /// </summary>
        public void DestroyData()
        {
            Destroy(this);
        }
    }
}
