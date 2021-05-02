using System.Collections.Generic;
using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.Helpers;
using Assets.Scripts.Game.Player;
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

        private Dictionary<int, (IRepositioner, PlayerReset)> _assignedPlayers = new Dictionary<int, (IRepositioner, PlayerReset)>();

        /// <summary>
        /// Registers new player in the base.
        /// </summary>
        /// <param name="playerID">Unique id of the player used to recognize them.</param>
        /// <param name="newPlayerPosReset">Script used to reset position of the player.</param>
        /// <param name="newPlayerReset">Script used to reset player's state.</param>
        public void AssignPlayer(int playerID, IRepositioner newPlayerPosReset, PlayerReset newPlayerReset)
        {
            newPlayerReset.RegisterOnPositionResetHandler(ResetPlayerPosition);
            _assignedPlayers.Add(playerID, (newPlayerPosReset, newPlayerReset));
        }
        /// <summary>
        /// Resets given player's position and only position.
        /// </summary>
        /// <param name="playerID">Unique id of player's instance used to recognize it (gameobject id).</param>
        public void ResetPlayerPosition(int playerID)
        {
            if (_assignedPlayers.TryGetValue(playerID, out var player))
            {
                (var repositioner, var state) = player;
                repositioner.ChangePosition(_playerSpawnPosition.position);
            }
        }
        /// <summary>
        /// Resets given player's position and state.
        /// </summary>
        /// <param name="playerID">Unique id of player's instance used to recognize it (gameobject id).</param>
        public void ResetPlayer(int playerID)
        {
            if(_assignedPlayers.TryGetValue(playerID, out var player))
            {
                (var repositioner, var state) = player;
                repositioner.ChangePosition(_playerSpawnPosition.position);
                state.ResetState();
            }
        }
        /// <summary>
        /// Repositions all of the players. All of them. WITHOUT resetting state.
        /// </summary>
        public void RepositionAllPlayers()
        {
            foreach (var player in _assignedPlayers.Values)
            {
                (var positioner, var stateReseter) = player;
                positioner.ChangePosition(_playerSpawnPosition.position);
            }
        }
    }
}
