using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Exceptions;
using Assets.Scripts.Common.Factories;
using Assets.Scripts.InspectorSerialization.Interfaces;
using UnityEditor;
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
        public List<GameObject> Players { get; private set; } = new List<GameObject>();
        /// <summary>
        /// List of all created spawners.
        /// </summary>
        public List<GameObject> Spawners { get; private set; } = new List<GameObject>();

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
            spawnerObjects.ForEach(s =>
            {
                s.transform.parent = _spawnersParent;
                Spawners.Add(s);
            });
        }
        /// <summary>
        /// Adds single spawn to the data structure.
        /// </summary>
        /// <param name="spawnerObject">New spawner object.</param>
        public void AddSpawner(GameObject spawnerObject)
        {
            Spawners.Add(spawnerObject);
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
