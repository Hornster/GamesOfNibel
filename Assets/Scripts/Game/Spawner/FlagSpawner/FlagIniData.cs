using Assets.Scripts.Game.Common.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Spawner.FlagSpawner
{
    /// <summary>
    /// Used to transfer data to freshly spawned flag, right from the spawner.
    /// </summary>
    public class FlagIniData
    {
        /// <summary>
        /// Callback for the flag - called when it gets reset because of staying on ground outside of any spawn for too long.
        /// </summary>
        public UnityAction FlagUnstuckSignal { get; set; }
        /// <summary>
        /// Signal from the flag that it has been just captured.
        /// </summary>
        public UnityAction FlagCapturedSignal { get; set; }
        /// <summary>
        /// The team that the yet-to-spawn flag should be of.
        /// </summary>
        public Teams FlagTeam { get; set; }
        /// <summary>
        /// The transform of the gameobject that defines spawn coords.
        /// </summary>
        public Transform FlagSpawnPosition { get; set; }
    }
}
