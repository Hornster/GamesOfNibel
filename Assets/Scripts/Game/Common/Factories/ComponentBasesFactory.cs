using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Data;
using Assets.Scripts.Game.Common.Data.Maps;
using Assets.Scripts.Game.Common.Data.ScriptableObjects.FactoryData;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Exceptions;
using Assets.Scripts.Game.Common.Helpers;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Factories
{
    public class ComponentBasesFactory : MonoBehaviour, IBasesFactory
    {
        [SerializeField]
        private ComponentFactoryDataSO _componentFactoryDataSO;
        [SerializeField]
        private TeamColorsSO _teamColorsSO;

        public List<GameObject> CreateSpawner(SpawnerGroupConfig spawnerGroupConfig)
        {
            var newBases = new List<GameObject>(spawnerGroupConfig.BasesData.Count);
            var basesData = spawnerGroupConfig.BasesData;

            foreach (var baseData in basesData)
            {
                var (newBase, baseComponents) = CreateBase(baseData);
                newBase = SetBaseColor(newBase, ref baseComponents, baseData.BaseTeam);
                newBase = SetBaseData(newBase, baseData);

                InjectMonoBehavioursIntoComponents(newBase, ref baseComponents);

                newBases.Add(newBase);
            }

            return newBases;
        }

        private (GameObject mainObject, GameObject[] baseComponents) CreateBase(BaseData baseData)
        {
            var baseMainObject = Instantiate(_componentFactoryDataSO.BaseMainPrefab);
            var baseFloor = _componentFactoryDataSO.CreateBaseFloor(baseData.BaseType);
            var baseSpire = _componentFactoryDataSO.CreateSpire(baseData.BaseType);
            var baseAdditionalElements = _componentFactoryDataSO.CreateAdditionalElements(baseData.BaseType);

            var baseComponents = new GameObject[EnumValueRetriever.GetEnumArray<BaseComponentTypeEnum>().Length];
            baseComponents[(int)BaseComponentTypeEnum.FloorObjectID] = baseFloor;
            baseComponents[(int)BaseComponentTypeEnum.SpireObjectID] = baseSpire;
            baseComponents[(int)BaseComponentTypeEnum.AdditionalElementsID] = baseAdditionalElements;

            if (baseFloor != null)
            {
                baseFloor.transform.parent = baseMainObject.transform;
            }
            if (baseSpire != null)
            {
                baseSpire.transform.parent = baseMainObject.transform;
            }
            if (baseAdditionalElements != null)
            {
                baseAdditionalElements.transform.parent = baseMainObject.transform;
            }

            return (baseMainObject, baseComponents);
        }

        private GameObject SetBaseColor(GameObject newBase, ref GameObject[] baseComponents, Teams team)
        {
            var baseColorController = newBase.GetComponentInChildren<BaseColorController>();
            if(baseColorController == null)
            {
                throw new GONBaseException("Could not find base color controller during base creation!");
            }

            baseColorController.SetTeamColorsSO(_teamColorsSO);
            var availableBaseComponents = EnumValueRetriever.GetEnumArray<BaseComponentTypeEnum>();

            foreach(var componentType in availableBaseComponents)
            {
                var currentBaseComponent = baseComponents[(int)componentType];
                if (currentBaseComponent != null)
                {
                    var colorSetters = currentBaseComponent.GetComponentsInChildren<ColorSetter>();
                    baseColorController.SetColorSettersList(colorSetters.ToList(), componentType);
                }
            }

            baseColorController.ChangeColor(team);

            return newBase;
        }

        private GameObject SetBaseData(GameObject newBase, BaseData baseData)
        {
            var teamModule = newBase.GetComponent<TeamModule>();

            teamModule.MyTeam = baseData.BaseTeam;

            var baseDataComponent = newBase.GetComponent<BaseDataGOAdapter>();
            baseDataComponent.ID = baseData.ID;
            baseDataComponent.BaseTeam = teamModule;
            baseDataComponent.BaseType = baseData.BaseType;
            baseDataComponent.GameMode = baseData.GameMode;

            return newBase;
        }

        private void InjectMonoBehavioursIntoComponents(GameObject baseObject, ref GameObject[] baseComponents)
        {
            foreach (var baseComponent in baseComponents)
            {
                if (baseComponent == null)
                {
                    continue;
                }

                var injectionHooks = baseComponent.GetComponentsInChildren <IInjectionHook>();
                foreach (var injectionHook in injectionHooks)
                {
                    injectionHook.InjectReferences(baseObject);
                }
            }
        }
    }
}
