using System;
using System.Linq;
using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.CustomCollections.DefaultCollectionsSerialization.Dictionary;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Helpers;
using Assets.Scripts.MapEdit.Common.Data.CustomContainers;
using Assets.Scripts.MapEdit.Editor.Data.Constants;
using UnityEngine;

namespace Assets.Scripts.MapEdit.Editor.Data.ScriptableObjects
{
    //At least one of the three components should be present.
    //The base marker should be visible, after all, in the editor.
    [CreateAssetMenu(fileName = BaseMarkerFactorySoName, menuName = SGMapEditConstants.MapEditMenuPath + BaseMarkerFactorySoName)]
    public class BaseMarkerFactorySO : ScriptableObject
    {
        public const string BaseMarkerFactorySoName = "BaseMarkerFactorySO";
        private const int _maxIDValue = 300000000;
        private int _lastUsedMarkerID = 0;


        [Header("Cache")]
        [Tooltip("Used to remember what base markers have been spawned.")]
        [SerializeField]
        private BaseMarkersCache _baseMarkersCache;
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

            var (baseMarker, appliedComponents) = CreateBaseMarker(floorPrefab, spirePrefab, additionsPrefab, position);
            baseMarker = SetBaseColorReferences(baseMarker, appliedComponents);
            baseMarker = SetBaseMarkerTeam(baseMarker, team);
            baseMarker = SetBaseMarkerData(baseMarker, gameplayMode, baseSubtype);

            //SaveBaseInCache(baseMarker);

            return baseMarker;
        }
        /// <summary>
        /// Connects color changer scripts in the applied components with the color changing controller in base marker.
        /// </summary>
        /// <param name="baseMarker"></param>
        /// <param name="appliedComponents"></param>
        /// <returns></returns>
        private GameObject SetBaseColorReferences(GameObject baseMarker, GameObject[] appliedComponents)
        {
            var baseColorController = baseMarker.GetComponent<BaseColorController>();
            var floorObjectID = (int)BaseComponentTypeEnum.FloorObjectID;
            var spireObjectID = (int)BaseComponentTypeEnum.SpireObjectID;
            var additionalElementsObjectID = (int) BaseComponentTypeEnum.AdditionalElementsID;

            if (appliedComponents[floorObjectID] != null)
            {
                var colorSetter = appliedComponents[floorObjectID].GetComponentsInChildren<ColorSetter>();
                baseColorController.BaseFloorColorSetter = colorSetter.ToList();
            }

            if (appliedComponents[spireObjectID] != null)
            {
                var colorSetters = appliedComponents[spireObjectID].GetComponentsInChildren<ColorSetter>();
                baseColorController.SpireColorSetter = colorSetters.ToList();
            }

            if (appliedComponents[additionalElementsObjectID] != null)
            {
                var colorSetters = appliedComponents[additionalElementsObjectID].GetComponentsInChildren<ColorSetter>();
                baseColorController.AdditionalElementsColorSetter = colorSetters.ToList();
            }

            var teamModule = baseMarker.GetComponent<TeamModule>();
            teamModule.RegisterOnTeamChangedHandler(baseColorController.ChangeColor);

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

        private (GameObject baseMarker, GameObject[] baseComponets) CreateBaseMarker(GameObject floorPrefab, GameObject spirePrefab, GameObject additionsPrefab, Vector3 position)
        {
            var baseMarker = Instantiate<GameObject>(_baseMainPrefab, position, Quaternion.identity);
            var appliedComponents = new GameObject[Enum.GetValues(typeof(BaseComponentTypeEnum)).Length];
            if (floorPrefab != null)
            {
                appliedComponents[(int)BaseComponentTypeEnum.FloorObjectID] = Instantiate(floorPrefab, baseMarker.transform);
            }

            if (spirePrefab != null)
            {
                appliedComponents[(int)BaseComponentTypeEnum.SpireObjectID] = Instantiate(spirePrefab, baseMarker.transform);
            }

            if (additionsPrefab != null)
            {
                appliedComponents[(int)BaseComponentTypeEnum.AdditionalElementsID] = Instantiate(additionsPrefab, baseMarker.transform);
            }

            return (baseMarker, appliedComponents);
        }

        private GameObject SetBaseMarkerTeam(GameObject baseMarker, Teams team)
        {
            var teamModule = baseMarker.GetComponentInChildren<TeamModule>();

            teamModule.MyTeam = team;

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
