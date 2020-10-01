using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Common.Data.NoDestroyOnLoad
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

        //TODO add character enum when char selection will become available

        
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
        /// Was the player already spawned by some previous spawner?
        /// </summary>
        public bool PlayerAlreadySpawned { get; set; }
    }
}
