using Assets.Scripts.Game.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Exceptions;
using Assets.Scripts.Game.Player.Character;
using UnityEngine;

namespace Assets.Scripts.Game.Spawner
{
    public class PlayerBases : MonoBehaviour
    {
        /// <summary>
        /// The team to which this base belongs.
        /// </summary>
        [SerializeField]
        private Teams _myTeam;
        [Tooltip("Prefab of the player used to spawn the players.")]
        [SerializeField]
        private GameObject _playerPrefab;
        [Tooltip("Transform at which the players will be spawned.")]
        [SerializeField] 
        private Transform _spawningPosition;
        /// <summary>
        /// Data about the match, including players that take part, the teams they belong to, allowed skills, etc.
        /// </summary>
        private MatchData _matchData;

        private void Start()
        {
            _matchData = MatchData.GetInstance();
            SpawnPlayers();
        }
        /// <summary>
        /// Creates instances of players.
        /// </summary>
        private void SpawnPlayers()
        {
            if (_matchData == null)
            {
                throw new GONBaseException($"Match data was null in {this}!");
            }

            var playersConfigs = _matchData.PlayersConfigs;

            if (playersConfigs == null)
            {
                throw new GONBaseException("No players found to spawn!");
            }

            var availablePlayerConfigs = playersConfigs.PlayerConfigs;
            for (int i = 0; i < availablePlayerConfigs.Count; i++)
            {
                var currentPlayerConfig = availablePlayerConfigs[i];
                SpawnSinglePlayer(currentPlayerConfig, _matchData.SkillsConfig);

            }
        }

        /// <summary>
        /// Spawns a single player, if possible.
        /// </summary>
        /// <param name="playerConfig">Data about the player that will be spawned.</param>
        /// <param name="skillsConfig">Which skills should be added to the player.</param>
        private void SpawnSinglePlayer(PlayerConfig playerConfig, SkillsConfig skillsConfig)
        {
            if (playerConfig.PlayerAlreadySpawned
                || (playerConfig.PlayerTeam != _myTeam && _myTeam != Teams.Multi))
            {
                return; //We do not care about a player that is already spawned or does not belong to our team, and our team is NOT multi.
            }

            var newPlayerInstance = Instantiate(_playerPrefab, _spawningPosition);
            var skillContainerProvider = newPlayerInstance.GetComponent<SkillContainerProvider>();
        }

        
        /// <summary>
        /// Respawns player of given ID - resets them and puts at the spawning transform.
        /// </summary>
        /// <param name="id"></param>
        private void RespawnPlayer(int id)
        {

        }
    }
}
