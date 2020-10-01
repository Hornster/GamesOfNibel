
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Spawner
{
    public class PlayerBase : MonoBehaviour
    {
        /// <summary>
        /// The team to which this base belongs.
        /// </summary>
        [SerializeField]
        private Teams _myTeam;
        [Tooltip("Prefab of the player used to spawn the players.")]
        [SerializeField]
        private GameObject _playerPrefab;

        private void Start()
        {
            SpawnPlayers();
        }
        public void SpawnPlayer()
        {
            
        }
        //TODO this won't do. Try to diverge player and flag spawners on higher abstraction level, like an interface.
    }
}
