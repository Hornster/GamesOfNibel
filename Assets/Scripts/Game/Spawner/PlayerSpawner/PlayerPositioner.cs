using System.Collections.Generic;
using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.Helpers;
using UnityEngine;

namespace Assets.Scripts.Game.Spawner.PlayerSpawner
{
    /// <summary>
    /// Used in bases. Keeps an eye for assigned players and repositions them to the base when necessary.
    /// </summary>
    public class PlayerPositioner : MonoBehaviour
    {
        [Tooltip("Where should the players be put after spawning/respawning.")]
        [SerializeField]
        private Transform _playerSpawnPosition;
        [Tooltip("Information about the base's team.")]
        [SerializeField]
        private TeamModule _spawnerTeam;

        private List<IRepositioner> _assignedPlayers = new List<IRepositioner>();

        /// <summary>
        /// Registers new player in the base.
        /// </summary>
        /// <param name="newPlayer"></param>
        public void AssignPlayer(IRepositioner newPlayer)
        {
            _assignedPlayers.Add(newPlayer);
        }
        /// <summary>
        /// Repositions all of the players. All of them.
        /// </summary>
        public void RepositionAllPlayers()
        {
            foreach (var player in _assignedPlayers)
            {
                player.ChangePosition(_playerSpawnPosition.position);
            }
        }
    }
}
