using System.Collections.Generic;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game.Common.Data.NoDestroyOnLoad
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
        public override Dictionary<Teams, List<GameObject>> Players { get; protected set; } = new Dictionary<Teams, List<GameObject>>();
        /// <summary>
        /// List of all created spawners.
        /// </summary>
        public override Dictionary<Teams, List<GameObject>> Bases { get; protected set; } = new Dictionary<Teams, List<GameObject>>();

        /// <summary>
        /// Adds player to the data structure.
        /// </summary>
        /// <param name="playerObject">New player object.</param>
        public void AddPlayer(GameObject playerObject)
        {
            var team = playerObject.GetComponentInChildren<TeamModule>().MyTeam;
            playerObject.transform.parent = _playersParent;
            if (Players.TryGetValue(team, out var playerList))
            {
                playerList.Add(playerObject);
            }
            else
            {
                var newPlayerList = new List<GameObject>{playerObject};
                Players.Add(team, newPlayerList);
            }
        }

        /// <summary>
        /// Adds multiple spawners to the data structure.
        /// </summary>
        /// <param name="spawnerObjects">Bases that should be added.</param>
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
            if (Bases.TryGetValue(team, out var spawnersList))
            {
                spawnersList.Add(spawnerObject);
            }
            else
            {
                var newSpawnersList = new List<GameObject> { spawnerObject };
                Bases.Add(team, newSpawnersList);
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
