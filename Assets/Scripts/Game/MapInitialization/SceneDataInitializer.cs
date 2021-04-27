using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.CustomEvents;
using Assets.Scripts.Game.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Game.Common.Exceptions;
using Assets.Scripts.Game.InspectorSerialization.Interfaces;
using Assets.Scripts.Game.Player.Character;
using Assets.Scripts.Game.Player.Character.Skills;
using UnityEngine;

namespace Assets.Scripts.Game.MapInitialization
{
    public class SceneDataInitializer : MonoBehaviour
    {
        [Header("Factories")]
        [Tooltip("Character factory object. Used to create player characters.")]
        [SerializeField]
        private ICharacterFactoryContainer _characterFactory;
        [Tooltip("Spawner factory object. Used to create player bases (spawners).")]
        [SerializeField]
        private ISpawnerFactoryContainer _spawnerFactory;
        [Tooltip("Provides skills for created characters.")]
        [SerializeField] 
        private ISkillsFactoryContainer _skillsFactory;

        [Header("Scene Deployment")]
        [Tooltip("Prefab of the SceneData object. Will contain ready for deployment dynamic objects for the loaded map.")]
        [SerializeField]
        private GameObject _sceneDataPrefab;
        [Tooltip("Assigns players to created bases and repositions them.")]
        [SerializeField]
        private PlayerAssigner _playerAssigner;
        [Tooltip("Contains base objects that every scene should have.")] 
        [SerializeField]
        private GameObject _baseSceneObjectsPrefab;

        [Header("Events")]
        [Tooltip("Called when the user confirmed selected map and its loading sequence has been started.")]
        [SerializeField] private StringUnityEvent _onSceneDataLoaded;

        /// <summary>
        /// Raw data concerning the match. Contains amount of spawns, players, etc.
        /// </summary>
        private MatchData _matchData;
        /// <summary>
        /// Ready objects for the map scene.
        /// </summary>
        private SceneData _sceneData;

        private bool _isDoneLoading = false;

        private object _isDoneLoadingLock = new object();
        public bool IsDoneLoading
        {
            get
            {
                lock (_isDoneLoadingLock)
                {
                    return _isDoneLoading;
                }
            }
        }

        //private void Start()
        //{            
        //    StartCoroutine(CreateData());
        //}

        public IEnumerator CreateData()
        {
            var newSceneDataObj = Instantiate(_sceneDataPrefab);
            Instantiate(_baseSceneObjectsPrefab, newSceneDataObj.transform);
            _sceneData = newSceneDataObj.GetComponentInChildren<SceneData>();
            DontDestroyOnLoad(newSceneDataObj);//This object is supposed to survive being passed to the loaded map.
            //Then, after retrieving its contents, we can destroy it.
            if (RetrieveDataObject() == false)
            {
                throw new GONBaseException("No matchdata found during map loading!");
            }
            CreateSpawners();
            CreatePlayers();

            _playerAssigner.PositionPlayers(_sceneData);

            lock (_isDoneLoadingLock)
            {
                _isDoneLoading = true;
            }

            yield return null;
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
            _onSceneDataLoaded.Invoke(_matchData.SceneToLoad);
        }
        /// <summary>
        /// Creates data that is needed for the map.
        /// </summary>
        /// <returns></returns>
        
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
            var spawnersData = _matchData.SpawnersConfig.SpawnerConfigs;
            foreach (var spawnerData in spawnersData)
            {
                var newSpawners = _spawnerFactory.Interface.CreateSpawner(spawnerData);
                _sceneData.AddSpawners(newSpawners);
            }
        }
        /// <summary>
        /// Creates players based off the matchData contents.
        /// </summary>
        private void CreatePlayers()
        {
            var playerConfigs = _matchData.PlayersConfigs.PlayerConfigs;
            foreach (var playerConfig in playerConfigs)
            {
                var newPlayer = _characterFactory.Interface.CreateCharacter(playerConfig);
                CreateSkillsForPlayer(newPlayer);
                _sceneData.AddPlayer(newPlayer);
            }
        }
        /// <summary>
        /// Adds skills to player accordingly with provided with match data skills configuration.
        /// </summary>
        /// <param name="playerObject"></param>
        private void CreateSkillsForPlayer(GameObject playerObject)
        {
            var skillInfoProvider = playerObject.GetComponentInChildren<SkillInfoProvider>();

            if (skillInfoProvider == null)
            {
                throw new GONBaseException("All characters have to have a skill info provider attached to them!");
            }

            var skillsConfig = _matchData.SkillsConfig.AvailableSkills.dictionary;
            var skillsFactory = _skillsFactory.Interface;
            foreach (var skillConfig in skillsConfig)
            {
                if (skillConfig.Value)
                {
                    //If the skill is allowed, add it to the player.
                    var newSkill = skillsFactory.CreateSkill(skillInfoProvider.SkillsContainerGO,
                        skillInfoProvider.PlayerState, skillInfoProvider.CharacterRigidbody, skillConfig.Key);

                    skillInfoProvider.SkillsController.AddBasicSkill(skillConfig.Key, newSkill);
                }
            }
        }
    }
}
