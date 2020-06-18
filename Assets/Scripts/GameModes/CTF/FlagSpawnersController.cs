﻿
using Assets.Scripts.Common;
using Assets.Scripts.Spawner;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.GameModes.CTF
{
    public class FlagSpawnersController
    {
        /// <summary>
        /// Array of available neutral flag spawners.
        /// </summary>
        [SerializeField]
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
            _isNeutralFlagSpawned = false;
            _neutralFlagRespawnTimer.Start();
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
                FlagUnstuckSignal = NeutralFlagResetHandler
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
        }

        private void FixedUpdate()
        {
            if (!_isNeutralFlagSpawned)
            {
                _neutralFlagRespawnTimer.Update();
            }
        }

        private void Start()
        {
            _neutralFlagRespawnTimer = new Timer(_flagRespawnTime, SpawnNeutralFlagHandler);
            _randomGenerator = new Random();
        }
    }
}