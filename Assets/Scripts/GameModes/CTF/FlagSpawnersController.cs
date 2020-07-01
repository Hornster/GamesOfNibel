
using Assets.Scripts.Common;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Spawner;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace Assets.Scripts.GameModes.CTF
{
    [RequireComponent(typeof(Timer))]
    public class FlagSpawnersController : MonoBehaviour
    {
        /// <summary>
        /// Array of available neutral flag spawners.
        /// </summary>
        private IFlagSpawner[] _neutralFlagSpawnerModules;
        
        /// <summary>
        /// The time it takes to create a new flag. Counting from most recent capture of the flag.
        /// </summary>
        [SerializeField]
        private float _flagRespawnTime;
        /// <summary>
        /// Indicates if neutral flag is already spawned.
        /// </summary>
        private bool _isNeutralFlagSpawned = false;
        /// <summary>
        /// Used to measure time of the next neutral flag spawn.
        /// </summary>
        private Timer _neutralFlagRespawnTimer;
        /// <summary>
        /// Used to select flag spawner.
        /// </summary>
        private Random _randomGenerator;
        /// <summary>
        /// Event handler for the event of capturing the neutral flag.
        /// </summary>
        private void NeutralFlagCapturedHandler()
        {
            Debug.Log("New flag will spawn in " + _flagRespawnTime + " seconds.");

            _isNeutralFlagSpawned = false;
            _neutralFlagRespawnTimer.StartTimer();
        }
        /// <summary>
        /// Called when the neutral flag has been reset back to the spawn.
        /// </summary>
        private void NeutralFlagResetHandler()
        {
            Debug.Log("Flag spawners controller here, mah flag's been reset.");
        }
        /// <summary>
        /// Prepares the flag data.
        /// </summary>
        /// <returns></returns>
        private FlagIniData PrepareFlagData()
        {
            var flagData = new FlagIniData()
            {
                FlagUnstuckSignal = NeutralFlagResetHandler,
                FlagCapturedSignal = NeutralFlagCapturedHandler
            };

            return flagData;
        }
        /// <summary>
        /// Called when it is the time for spawning new neutral flag.
        /// </summary>
        private void SpawnNeutralFlagHandler()
        {
            _isNeutralFlagSpawned = true;
            _neutralFlagRespawnTimer.ResetAndStop();
            int whichSpawn = _randomGenerator.Next(0, _neutralFlagSpawnerModules.Length);

            _neutralFlagSpawnerModules[whichSpawn].SpawnEntity(PrepareFlagData());

            Debug.Log("Neutral flag respawned.");
        }
        
        private void Start()
        {
            _neutralFlagSpawnerModules = GetComponentsInChildren<IFlagSpawner>();
            _neutralFlagRespawnTimer = GetComponent<Timer>();
            _neutralFlagRespawnTimer.RegisterTimeoutHandler(SpawnNeutralFlagHandler);
            _neutralFlagRespawnTimer.MaxAwaitTime = _flagRespawnTime;
            _randomGenerator = new Random();

            NeutralFlagCapturedHandler();
        }
    }
}
