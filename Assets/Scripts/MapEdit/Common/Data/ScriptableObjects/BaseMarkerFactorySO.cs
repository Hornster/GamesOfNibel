using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.CustomCollections.DefaultCollectionsSerialization.Dictionary;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Exceptions;
using Assets.Scripts.MapEdit.Common.Data.CustomContainers;
using UnityEngine;

namespace Assets.Scripts.MapEdit.Common.Data.ScriptableObjects
{
    //At least one of the three components should be present.
    //The base marker should be visible, after all, in the editor.
    [CreateAssetMenu(fileName = BaseMarkerFactorySoName, menuName = "ScriptableObjects/MapEdit/" + BaseMarkerFactorySoName)]
    public class BaseMarkerFactorySO : ScriptableObject
    {
        public const string BaseMarkerFactorySoName = "BaseMarkerFactorySO";
        public const string PathToBaseMarkerFactorySo = "Assets/Sandbox/ComponentBasesTests/MapEdit/Data/SOs";
        private const int _maxIDValue = 300000000;
        private int _lastUsedMarkerID = 0;

        [Header("Base marker main object")]
        [Tooltip("The main base marker object which contains the components and universal scripts.")]
        [SerializeField]
        private GameObject _baseMainPrefab;
        [Header("Base markers components")]
        [SerializeField]
        private BaseSubtypeGameObjectDictionary _baseFloorComponents = new BaseSubtypeGameObjectDictionary();
        [SerializeField]
        private BaseSubtypeGameObjectDictionary _baseSpireComponents = new BaseSubtypeGameObjectDictionary();
        [Tooltip("Additional components that can be assigned to given base, like spawn areas.")]
        [SerializeField]
        private BaseSubtypeGameObjectDictionary _baseAdditionalComponents = new BaseSubtypeGameObjectDictionary();

        public GameObject CreateBaseMarker(Teams team, GameplayModesEnum gameplayMode, BaseSubtypeEnum baseSubtype, Vector3 position)
        {
            var floorPrefab = GetPrefabFromDictionary(gameplayMode, baseSubtype, _baseFloorComponents);
            var spirePrefab = GetPrefabFromDictionary(gameplayMode, baseSubtype, _baseSpireComponents);
            var additionsPrefab = GetPrefabFromDictionary(gameplayMode, baseSubtype, _baseAdditionalComponents);

            if (floorPrefab == null && spirePrefab == null && additionsPrefab == null)
            {
                throw new Exception("The marker base must have one of the three: a floor, a spire or additional components group. Otherwise it would be invisible in the editor!");
            }

            var baseMarker = CreateBaseMarker(floorPrefab, spirePrefab, additionsPrefab, position);
            baseMarker = SetBaseMarkerTeam(baseMarker, team);
            baseMarker = SetBaseMarkerData(baseMarker, gameplayMode, baseSubtype);

            return baseMarker;
        }
        /// <summary>
        /// Tries to find prefab. If manages to - returns it. Returns null otherwise.
        /// </summary>
        /// <param name="gameplayMode"></param>
        /// <param name="baseSubtype"></param>
        /// <param name="dictionary">Which of the available dictionary to query to.</param>
        /// <returns></returns>
        private GameObject GetPrefabFromDictionary(GameplayModesEnum gameplayMode, BaseSubtypeEnum baseSubtype, BaseSubtypeGameObjectDictionary dictionary)
        {
            GameObject returnValue = null;
            var key = new GameplayModeBaseSubtypeTuple(gameplayMode, baseSubtype);
            if (dictionary.dictionary.TryGetValue(key, out var prefab))
            {
                returnValue = prefab;
            }

            return returnValue;
        }

        private GameObject CreateBaseMarker(GameObject floorPrefab, GameObject spirePrefab, GameObject additionsPrefab, Vector3 position)
        {
            var baseMarker = Instantiate<GameObject>(_baseMainPrefab, position, Quaternion.identity);
            if (floorPrefab != null)
            {
                Instantiate(floorPrefab, baseMarker.transform);
            }

            if (spirePrefab != null)
            {
                Instantiate(spirePrefab, baseMarker.transform);
            }

            if (additionsPrefab != null)
            {
                Instantiate(additionsPrefab, baseMarker.transform);
            }

            return baseMarker;
        }

        private GameObject SetBaseMarkerTeam(GameObject baseMarker, Teams team)
        {
            var teamModules = baseMarker.GetComponentsInChildren<TeamModule>();

            foreach (var teamModule in teamModules)
            {
                teamModule.MyTeam = team;
            }

            return baseMarker;
        }

        private int GetNewBaseMarkerID()
        {
            _lastUsedMarkerID += 1;
            if (_lastUsedMarkerID > _maxIDValue)
            {
                _lastUsedMarkerID = 0;
            }

            return _lastUsedMarkerID;
        }
        private GameObject SetBaseMarkerData(GameObject baseMarker, GameplayModesEnum gameplayMode, BaseSubtypeEnum baseSubtype)
        {
            var baseMarkerData = baseMarker.GetComponent<BaseMarkerData>();

            if (baseMarkerData == null)
            {
                throw new Exception("No base marker data found in base main prefab!");
            }

            baseMarkerData.ID = GetNewBaseMarkerID();
            baseMarkerData.GameMode = gameplayMode;
            baseMarkerData.BaseSubtype = baseSubtype;

            return baseMarker;
        }
    }
}
