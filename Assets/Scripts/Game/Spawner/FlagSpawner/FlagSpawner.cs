using Assets.Scripts.Common;
using Assets.Scripts.GameModes.CTF.Entities;
using UnityEngine;

namespace Assets.Scripts.Spawner.FlagSpawner
{
    public class FlagSpawner : MonoBehaviour, IFlagSpawner
    {

        /// <summary>
        /// Stores info about the team this spawn belongs to.
        /// </summary>
        [SerializeField] private TeamModule _teamModule;
        /// <summary>
        /// The entity that this spawner spawns.
        /// </summary>
        [SerializeField]
        private GameObject _spawningEntityPrefab;
        /// <summary>
        /// Where the new objects shall be spawned.
        /// </summary>
        [SerializeField]
        private Transform _spawningPosition;
        /// <summary>
        /// Called by the flag that belongs to this spawn when it has been unstuck.
        /// </summary>
        private void FlagHasBeenUnstuck()
        {
            Debug.Log($"Says spawner {this}: Neutral flag has been unstuck!");
        }

        /// <summary>
        /// Called to spawn the flag at _spawningPosition.
        /// </summary>
        /// <param name="flagIniData">Team of the flag.</param>
        public void SpawnEntity(FlagIniData flagIniData)
        {
            var newFlag = Instantiate(_spawningEntityPrefab, _spawningPosition.position, Quaternion.identity);
            var flagController = newFlag.GetComponentInChildren<FlagController>();
            flagIniData.FlagUnstuckSignal += FlagHasBeenUnstuck;
            flagIniData.FlagTeam = _teamModule.MyTeam;
            flagIniData.FlagSpawnPosition = _spawningPosition;
            flagController.SetFlagData(flagIniData);
        }
    }
}
