using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Helpers;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MapEdit.Editor.Data.ScriptableObjects
{
    public class BaseMarkersCache : UnityEditor.Editor
    {
        private List<BaseMarkerData> _baseDatas = new List<BaseMarkerData>();

        private static BaseMarkersCache _instance;
        /// <summary>
        /// Set to true if a base was added through a script.
        /// </summary>
        public bool BasesAdded { get; set; } = true;

        /// <summary>
        /// Searches for any objects with <see cref="BaseMarkerData"/> component attached to them.
        /// Is called automatically upon retrieving bases data via <see cref="GetBasesData"/> if
        /// <see cref="BasesAdded"/> is set to true.
        /// </summary>

        public static BaseMarkersCache GetInstance()
        {
            if (_instance == null)
            {
                _instance = UnityEditor.Editor.CreateInstance<BaseMarkersCache>();
                EditorSceneManager.activeSceneChangedInEditMode += _instance.OnSceneChanged;
            }

            return _instance;
        }

        public void OnDestroy()
        {
            _instance = null;
        }
        public void FindBases()
        {
            var foundBaseMarkers = FindObjectsOfType<BaseMarkerData>();
            _baseDatas = foundBaseMarkers.ToList();
            BasesAdded = false;
        }
        /// <summary>
        /// Retrieves data about bases. If <see cref="BasesAdded"/> is set to true - checks
        /// for new bases. If not, checks if any of the bases have been removed before returning.
        /// </summary>
        /// <returns>List of data of bases.</returns>
        public List<BaseMarkerData> GetBasesData()
        {
            if (BasesAdded)
            {
                FindBases();
            }
            else
            {
                ChkReferences();
            }

            return _baseDatas;
        }

        public Dictionary<Teams, int> GetBasesCountByTeam()
        {
            var basesCount = new Dictionary<Teams, int>();
            var teamsTypes = EnumValueRetriever.GetEnumArray<Teams>();
            foreach (var teamType in teamsTypes)
            {
                basesCount.Add(teamType, 0);
            }

            foreach (var baseData in _baseDatas)
            {
                basesCount[baseData.BaseTeam]++;
            }

            return basesCount;
        }

        private void OnSceneChanged(Scene previousScene, Scene currentScene)
        {
            BasesAdded = true; //We treat new scene as if new bases were added - we need to search for the bases again after all.
        }
        private void ChkReferences()
        {
            for (int i = _baseDatas.Count - 1; i >= 0; i--)
            {
                if (_baseDatas[i] == null)
                {
                    _baseDatas.RemoveAt(i);
                }
            }
        }
    }
}
