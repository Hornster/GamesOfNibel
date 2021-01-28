using System.Collections.Generic;
using Assets.MapEdit.Scripts;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Exceptions;
using UnityEngine;

namespace Assets.Scripts.Common.Data.Maps
{
    public class BasesRoot : MonoBehaviour
    {
        [Tooltip("Parent gameobject that holds the bases' markers.")]
        [SerializeField]
        private GameObject BasesParent;
        /// <summary>
        /// Seeks for bases in BasesParent gameobject, counts them and returns
        /// dictionary with info how many bases of what teams are there.
        /// </summary>
        /// <returns></returns>
        public Dictionary<Teams, int> GetBasesCount()
        {
            var dictionary = new Dictionary<Teams, int>();

            var playerBases = BasesParent.GetComponentsInChildren<SpawnerMarkerScript>();
            if (playerBases == null)
            {
                throw new GONBaseException("There are no base markers in the bases parent object! Please add a team Lily, Lotus or Multi team base marker.");
            }
            foreach(var playerBase in playerBases)
            {
                var teamModule = playerBase.GetComponentInChildren<TeamModule>();
                if (teamModule != null)
                {
                    AddBaseMarker(dictionary, teamModule.MyTeam);
                }
            }

            return dictionary;
        }

        private void AddBaseMarker(Dictionary<Teams, int> markersDict, Teams markerTeam)
        {
            if (markersDict.TryGetValue(markerTeam, out int count))
            {
                markersDict[markerTeam] = count++;
            }
            else
            {
                markersDict.Add(markerTeam, 1);
            }
        }
    }
}
