using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Data.NoDestroyOnLoad
{
    /// <summary>
    /// Configuration of the player performed in the lobby.
    /// </summary>
    public class PlayerConfig : MonoBehaviour
    {
        //The fields and getters/setters are explicitly divided in order to allow
        //testing through Unity Inspector.
        /// <summary>
        /// Name of the player.
        /// </summary>
        [Tooltip("Name of the player.")]
        [SerializeField]
        private string _playerName;
        /// <summary>
        /// The team the player belongs to.
        /// </summary>
        [Tooltip("The team the player belongs to.")]
        [SerializeField]
        private Teams _playerTeam;
        [Tooltip("Defines whether is this first or second player, or perhaps a client connected to server. First and Second players are treated as hosts.")]
        [SerializeField]
        private WhatPlayer _whatPlayer;

        [Tooltip("Id of the player. MUST BE UNIQUE!")]
        [SerializeField]
        private int _myId;
        //TODO add character enum when char selection will become available
        /// <summary>
        /// Id of the player.
        /// </summary>
        public int MyId => _myId;
        /// <summary>
        /// The name the player decided to hide behind.
        /// </summary>
        public string PlayerName
        {
            get => _playerName;
            set => _playerName = value;
        }
        /// <summary>
        /// The team the player belongs to.
        /// </summary>
        public Teams PlayerTeam
        {
            get => _playerTeam;
            set => _playerTeam = value;
        }
        /// <summary>
        /// Which player is this - P1? P2? (split-screen) Client? (network)
        /// </summary>
        public WhatPlayer WhatPlayer
        {
            get => _whatPlayer;
            set => _whatPlayer = value;
        }
        /// <summary>
        /// Was the player already spawned by some previous spawner?
        /// </summary>
        public bool PlayerAlreadySpawned { get; set; }
        /// <summary>
        /// Sets id of the player to new one.
        /// </summary>
        /// <param name="newId"></param>
        public void SetPlayerId(int newId)
        {
            _myId = newId;
        }
        /// <summary>
        /// Destroys this config's gameobject.
        /// </summary>
        public void DestroyConfig()
        {
            if (this.gameObject != null)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
