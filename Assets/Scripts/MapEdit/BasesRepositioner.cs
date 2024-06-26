﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.Data.Maps;
using Assets.Scripts.Game.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Exceptions;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.MapEdit
{
    /// <summary>
    /// Knows about every single spawn marker on the map.
    /// </summary>
    public class BasesRepositioner : MonoBehaviour
    {
        [Tooltip("The base gameobject that is a parent of all bases markers.")]
        [SerializeField]
        private GameObject BaseMarkersRoot;
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
            ChkSpawnersCountAgainstMarkers();
            HideMarkers();
        }
        /// <summary>
        /// Hides all markers on the scene.
        /// </summary>
        private void HideMarkers()
        {
            BaseMarkersRoot.SetActive(false);
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
        /// Checks if the amount of spawnersByTeam, grouped by teams, is equal to amount of
        /// markers, grouped by team. If not - yeets an exception of GONBaseException type.
        /// </summary>
        private void ChkSpawnersCountAgainstMarkers()
        {
            var spawners = _sceneData.Bases;
            var possibleSpawnerTypes = GetTeams();

            if (_foundSpawnMarkers.Count <= 0)
            {
                throw new GONBaseException($"No spawn markers found! At least one spawn shall be included in the map!");
            }

            if (spawners.Count != _foundSpawnMarkers.Count)
            {
                throw new GONBaseException($"Bases and markers types count misalignment! \n" +
                                           $"Spawner types: {spawners.Count}, \n" +
                                           $"Markers types: {_foundSpawnMarkers.Count}");
            }

            foreach (var team in possibleSpawnerTypes)
            {
                List<GameObject> spawnersList;
                Queue<ISpawnerMarker> spawnerMarkers;

                spawners.TryGetValue(team, out spawnersList);
                _foundSpawnMarkers.TryGetValue(team, out spawnerMarkers);

                if (spawnersList == null && spawnerMarkers == null)
                {
                    continue; //We do not have anything to check here.
                }

                if (spawnersList == null)
                {   //If we got here, then there are markers but no spawnersByTeam. Inform about this.
                    throw new GONBaseException($"No spawners found for team {team}! Expected count: {spawnerMarkers.Count}");
                }

                if (spawnerMarkers == null)
                {   //If we got here, then there are spawnersByTeam but no markers. Inform about this.
                    throw new GONBaseException($"No markers found for team {team}! Current spawns count: {spawnersList.Count}");
                }

                if (spawnerMarkers.Count != spawnersList.Count)
                {
                    //If we got here, then it means that the amount of spawnersByTeam doesn't equal markers count.
                    throw new GONBaseException($"Different amount of markers than spawns for team {team}! \n" +
                                        $"Bases found: {spawnersList.Count} \n" +
                                        $"Markers found: {spawnerMarkers.Count}");
                }
                RepositionSpawns(spawnersList, spawnerMarkers);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Teams[] GetTeams()
        {
            return Enum.GetValues(typeof(Teams)) as Teams[];
        }
        /// <summary>
        /// Positions available spawns on the markers' positions. Passed parameters have to be grouped
        /// by the same team!
        /// </summary>
        /// <param name="spawnersByTeam">All spawners that have the same team.</param>
        /// <param name="spawnerMarkers">Markers for all spawners of given team.</param>
        private void RepositionSpawns([NotNull] List<GameObject> spawnersByTeam, [NotNull] Queue<ISpawnerMarker> spawnerMarkers)
        {
            while(spawnerMarkers.Count != 0)
            {
                var nextSpawnerMarker = spawnerMarkers.Dequeue();
                var spawnerMarkerData = nextSpawnerMarker.MarkerTransform.GetComponent<BaseMarkerData>();
                foreach(var spawner in spawnersByTeam)
                {
                    var spawnerData = spawner.GetComponent<BaseDataGOAdapter>();
                    if(spawnerData.ID == spawnerMarkerData.ID)
                    {
                        if (nextSpawnerMarker.MoveSpawn(spawner) == false)
                        {
                            throw new Exception($"Bases of team {nextSpawnerMarker.SpawnerTeam} are overlapping!");
                        }
                        break;
                    }
                }
            }
            //foreach (var spawner in spawnersByTeam)
            //{
            //    var spawnerTeam = spawner.GetComponentInChildren<TeamModule>().MyTeam;
            //    if (_foundSpawnMarkers.TryGetValue(spawnerTeam, out var markers))
            //    {
            //        var marker = markers.Dequeue();
            //        if (marker.MoveSpawn(spawner) == false)
            //        {
            //            throw new Exception($"Bases of team {spawnerTeam} are overlapping!");
            //        }
            //    }
            //    else
            //    {
            //        throw new Exception($"Tried to add spawn of team that was not present on the map: {spawnerTeam}!");
            //    }
            //}

        }
    }
}
