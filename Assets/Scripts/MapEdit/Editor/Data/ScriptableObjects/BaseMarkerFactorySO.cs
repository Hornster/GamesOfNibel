using System;
using System.Linq;
using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.CustomCollections.DefaultCollectionsSerialization.Dictionary;
using Assets.Scripts.Game.Common.Data;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Helpers;
using Assets.Scripts.MapEdit.Editor.Data.Constants;
using Unity.Collections;
using UnityEditor;
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


        [SerializeField]
        [HideInInspector]
        private int _lastUsedMarkerID;

        [Header("Data Sources")]
        [SerializeField]
        private TeamColorsSO _teamColorsSO;
        [Header("Cache")]
        [Tooltip("Used to remember what base markers have been spawned.")]
        [SerializeField]
        private BaseMarkersCache _baseMarkersCache;
        [Header("Base marker main object")]
        [Tooltip("The main base marker object which contains the components and universal scripts.")]
        [SerializeField]
        private GameObject _baseMainPrefab;
        [Header("Root prefab")]
        [Tooltip("All of generated bases will be stored in here.")]
        private GameObject _basesRootPrefab;
        [Header("Base markers components")]
        [SerializeField]
        private BaseTypeGameObjectDictionary _baseFloorComponents = new BaseTypeGameObjectDictionary();
        [SerializeField]
        private BaseTypeGameObjectDictionary _baseSpireComponents = new BaseTypeGameObjectDictionary();
        [Tooltip("Additional components that can be assigned to given base, like spawn areas.")]
        [SerializeField]
        private BaseTypeGameObjectDictionary _baseAdditionalComponents = new BaseTypeGameObjectDictionary();

        private void Awake()
        {
            Debug.Log($"Last used base ID: {_lastUsedMarkerID}");
        }
        /// <summary>
        /// Creates and returns an object that should contain all bases.
        /// </summary>
        /// <returns></returns>
        public GameObject CreateBaseRoot()
        {
            return Instantiate(_basesRootPrefab);
        }
        public GameObject CreateBaseMarker(Teams team, BaseTypeEnum baseType, Vector3 position)
        {
            var floorPrefab = GetPrefabFromDictionary(baseType, _baseFloorComponents);
            var spirePrefab = GetPrefabFromDictionary(baseType, _baseSpireComponents);
            var additionsPrefab = GetPrefabFromDictionary(baseType, _baseAdditionalComponents);

            if (floorPrefab == null && spirePrefab == null && additionsPrefab == null)
            {
                throw new Exception("The marker base must have one of the three: a floor, a spire or additional components group. Otherwise it would be invisible in the editor!");
            }

            var (baseMarker, appliedComponents) = CreateBaseMarker(floorPrefab, spirePrefab, additionsPrefab, position);
            baseMarker = SetBaseColorReferences(baseMarker, appliedComponents);
            baseMarker = SetBaseMarkerTeam(baseMarker, team);
            baseMarker = SetBaseMarkerData(baseMarker, baseType);

            baseMarker.name = CreateBaseName(team, baseType, baseMarker.name);

            return baseMarker;
        }

        private string CreateBaseName(Teams team, BaseTypeEnum baseType, string defaultBaseName)
        {
            defaultBaseName = defaultBaseName.Replace("(Clone)", string.Empty);
            return team.ToString() + baseType.ToString() + defaultBaseName;
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
            baseColorController.SetTeamColorsSO(_teamColorsSO);
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
        /// <param name="baseType"></param>
        /// <param name="dictionary">Which of the available dictionary to query to.</param>
        /// <returns></returns>
        private GameObject GetPrefabFromDictionary(BaseTypeEnum baseType, BaseTypeGameObjectDictionary dictionary)
        {
            GameObject returnValue = null;
            
            if (dictionary.dictionary.TryGetValue(baseType, out var prefab))
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

            EditorUtility.SetDirty(this);

            return _lastUsedMarkerID;
        }
        private GameObject SetBaseMarkerData(GameObject baseMarker, BaseTypeEnum baseType)
        {
            var gameplayMode = EnumValueRetriever.GetGameplayModeFromBaseType(baseType);
            var baseMarkerData = baseMarker.GetComponent<BaseMarkerData>();

            if (baseMarkerData == null)
            {
                throw new Exception("No base marker data found in base main prefab!");
            }

            baseMarkerData.ID = GetNewBaseMarkerID();
            baseMarkerData.GameMode = gameplayMode;
            baseMarkerData.BaseType = baseType;

            return baseMarker;
        }
    }
}
