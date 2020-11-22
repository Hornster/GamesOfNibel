using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.CustomEvents;
using Assets.Scripts.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Common.Exceptions;
using Assets.Scripts.InspectorSerialization.Interfaces;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Assets.Scripts.MapInitialization
{
    public class SceneDataInitializer : MonoBehaviour
    {
        [Tooltip("Character factory object. Used to create player characters.")]
        [SerializeField]
        private ICharacterFactoryContainer _characterFactory;
        [Tooltip("Spawner factory object. Used to create player bases (spawners).")]
        [SerializeField]
        private ISpawnerFactoryContainer _spawnerFactory;
        [Tooltip("Prefab of the SceneData object. Will contain ready for deployment dynamic objects for the loaded map.")]
        [SerializeField]
        private GameObject _sceneDataPrefab;
        [Tooltip("Called when the user confirmed selected map and its loading sequence has been started.")]
        [SerializeField] private StringUnityEvent _onMapLaunching;
        /// <summary>
        /// Raw data concerning the match. Contains amount of spawns, players, etc.
        /// </summary>
        private MatchData _matchData;
        /// <summary>
        /// Ready objects for the map scene.
        /// </summary>
        private SceneData _sceneData;

        private bool _isDoneLoading = false;

        private object _isDoneLoadingLock;

        private void Start()
        {            
            StartCoroutine(CreateData());
        }

        private void Update()
        {
            lock (_isDoneLoadingLock)
            {
                if (_isDoneLoading)
                {
                    LaunchMap();
                }
            }
        }

        private void LaunchMap()
        {
            //No null checking. It HAS to be called.
            _onMapLaunching.Invoke(_matchData.SceneToLoad);
        }
        /// <summary>
        /// Creates data that is needed for the map.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CreateData()
        {
            var newSceneDataObj = Instantiate(_sceneDataPrefab);
            _sceneData = newSceneDataObj.GetComponentInChildren<SceneData>();
            DontDestroyOnLoad(newSceneDataObj);//This object is supposed to survive being passed to the loaded map.
                                                //Then, after retrieving its contents, we can destroy it.
            if (RetrieveDataObject() == false)
            {
                throw new GONBaseException("No matchdata found during map loading!");
            }
            CreateSpawners();
            CreatePlayers();

            lock (_isDoneLoadingLock)
            {
                _isDoneLoading = true;
            }
            yield return null;
        }
        /// <summary>
        /// Tries to find the MatchData object.
        /// </summary>
        /// <returns></returns>
        private bool RetrieveDataObject()
        {
            _matchData = FindObjectOfType<MatchData>();

            return _matchData != null;
        }
        /// <summary>
        /// Creates spawners based off the matchData contents.
        /// </summary>
        private void CreateSpawners()
        {
            var readySpawners = new List<GameObject>();
            var spawnersData = _matchData.SpawnersConfig.SpawnerConfigs;
            foreach (var spawnerData in spawnersData)
            {
                var newSpawners = _spawnerFactory.Interface.CreateSpawner(spawnerData);
                newSpawners.ForEach(s => readySpawners.Add(s));
            }

            _sceneData.Spawners = readySpawners;
        }
        /// <summary>
        /// Creates players based off the matchData contents.
        /// </summary>
        private void CreatePlayers()
        {
            var readyPlayers = new List<GameObject>();
            var playerConfigs = _matchData.PlayersConfigs.PlayerConfigs;
            foreach (var playerConfig in playerConfigs)
            {
                var newPlayer = _characterFactory.Interface.CreateCharacter(playerConfig);
                readyPlayers.Add(newPlayer);
            }

            _sceneData.Players = readyPlayers;
        }

    }
}
