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
    public class SceneData : MonoBehaviour, ISceneData
    {
        [Tooltip("Character factory object. Used to create player characters.")]
        [SerializeField]
        private ICharacterFactoryContainer _characterFactory;
        [Tooltip("Spawner factory object. Used to create player bases (spawners).")]
        [SerializeField]
        private ISpawnerFactoryContainer _spawnerFactory;
        /// <summary>
        /// List of all created players.
        /// </summary>
        private List<GameObject> _players;
        /// <summary>
        /// List of all created spawners.
        /// </summary>
        private List<GameObject> _spawners;
        private MatchData _matchData;

        private void Start()
        {
            DontDestroyOnLoad(this);//This object is supposed to survive being passed to the loaded map.
            //Then we can destroy it.
            if (RetrieveDataObject() == false)
            {
                throw new GONBaseException("No matchdata found during map loading!");
            }
            CreateSpawners();
            CreatePlayers();
        }

        private bool RetrieveDataObject()
        {
            _matchData = FindObjectOfType<MatchData>();

            return _matchData != null;
        }

        private void CreateSpawners()
        {
            var spawnersData = _matchData.SpawnersConfig;
        }
        private void CreatePlayers()
        {
            var playerConfigs = _matchData.PlayersConfigs;
        }

        /// <summary>
        /// Marks this object to be destroyed.
        /// </summary>
        public void DestroyData()
        {
            Destroy(this);
        }
        public GameObject[] GetPlayers()
        {
            return _players.ToArray();
        }

        public GameObject[] GetSpawns()
        {
            return _spawners.ToArray();
        }
    }
}
