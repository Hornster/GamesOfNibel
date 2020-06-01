
using Assets.Scripts.Common.Enums;
using Assets.Scripts.GameModes.CTF.Entities;
using UnityEngine;

namespace Assets.Scripts.Spawner
{
    public class FlagSpawner : MonoBehaviour, ISpawner
    {
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

        }

        /// <summary>
        /// Called to spawn the flag at _spawningPosition.
        /// </summary>
        /// <param name="flagIniData">Team of the flag.</param>
        public void SpawnEntity(FlagIniData flagIniData)
        {
            var newFlag = Instantiate(_spawningEntityPrefab, _spawningPosition);
            var flagController = newFlag.GetComponentInChildren<FlagController>();
            flagIniData.FlagUnstuckSignal += FlagHasBeenUnstuck;
            flagController.SetFlagData(flagIniData);
        }
    }
}
