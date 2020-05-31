
using Assets.Scripts.Common.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Spawner
{
    /// <summary>
    /// Used to transfer data to freshly spawned flag, right from the spawner.
    /// </summary>
    public class FlagIniData
    {
        /// <summary>
        /// Will be called by the flag when new flag should be spawned. For example, when the current one has been captured.
        /// </summary>
        public UnityAction FlagRespawnSignal { get; set; }
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
