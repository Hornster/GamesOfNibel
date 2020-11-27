﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Exceptions;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Assets.MapEdit.Scripts
{
    /// <summary>
    /// Knows about every single spawn marker on the map.
    /// </summary>
    public class SpawnerPositioner : MonoBehaviour
    {
        /// <summary>
        /// Stores found markers. Bool value indicates whether has given marker had already assigned its own spawn. 
        /// </summary>
        private Dictionary<Teams, Queue<ISpawnerMarker>> _foundSpawnMarkers = new Dictionary<Teams, Queue<ISpawnerMarker>>();
        /// <summary>
        /// Reference to the scene data that contains all dynamically loaded data, like spawns.
        /// </summary>
        private ISceneData _sceneData;
        private void Start()
        {
            _sceneData = FindObjectOfType<ISceneData>();
            if (_sceneData == null)
            {
                throw new Exception("Error - no scene data found!");

            }
            var foundSpawnerMarkers = GetComponentsInChildren<ISpawnerMarker>();
            GroupFoundSpawns(foundSpawnerMarkers);
            RepositionSpawns();
        }
        /// <summary>
        /// Groups spawn markers by their teams.
        /// </summary>
        /// <param name="spawnerMarkers">All found spawn markers.</param>
        private void GroupFoundSpawns(ISpawnerMarker[] spawnerMarkers)
        {
            foreach (var spawnerMarker in spawnerMarkers)
            {
                if (_foundSpawnMarkers.TryGetValue(spawnerMarker.SpawnerTeam.MyTeam, out var markersGroup))
                {
                    markersGroup.Enqueue(spawnerMarker);
                }
                else
                {
                    var newMarkersGroup = new Queue<ISpawnerMarker>();
                    newMarkersGroup.Enqueue(spawnerMarker);
                    _foundSpawnMarkers.Add(spawnerMarker.SpawnerTeam.MyTeam, newMarkersGroup);
                }
            }
        }
        /// <summary>
        /// Positions available spawns on the markers' positions.
        /// </summary>
        private void RepositionSpawns()
        {
            var spawners = _sceneData.Spawners;
            foreach (var spawner in spawners)
            {
                var spawnerTeam = spawner.GetComponentInChildren<TeamModule>().MyTeam;
                if (_foundSpawnMarkers.TryGetValue(spawnerTeam, out var markers))
                {
                    if (markers.Count <= 0)
                    {
                        throw new Exception($"Incorrect amount of {spawnerTeam} spawns!");
                    }

                    var marker = markers.Dequeue();
                    if (marker.MoveSpawn(spawner) == false)
                    {
                        throw new Exception($"Spawners of team {spawnerTeam} are overlapping!");
                    }
                }
                else
                {
                    throw new Exception($"Tried to add spawn of team that was not present on the map: {spawnerTeam}!");
                }
            }
        }
    }
}