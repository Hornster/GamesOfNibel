using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Data;
using Assets.Scripts.Game.Common.Data.ScriptableObjects.FactoryData;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Factories
{
    public class ComponentBasesFactory : MonoBehaviour, IBasesFactory
    {
        [SerializeField]
        private ComponentFactoryDataSO _componentFactoryDataSO;
        public List<GameObject> CreateSpawner(SpawnerGroupConfig spawnerGroupConfig)
        {
            var newBases = new List<GameObject>(spawnerGroupConfig.BasesData.Count);
            var basesData = spawnerGroupConfig.BasesData;

            foreach(var baseData in basesData)
            {
                var baseMainObject = Instantiate(_componentFactoryDataSO.BaseMainPrefab);
                var baseFloor = CreateBaseFloor(baseData.BaseTeam, baseData.BaseType);
            }

            throw new NotImplementedException();
        }

        private GameObject CreateBaseFloor(Teams baseTeam, BaseTypeEnum baseType)
        {
            GameObject baseFloor = null;
            var baseFloorDict = _componentFactoryDataSO.BaseFloorComponents.dictionary;
            if(baseFloorDict.TryGetValue(baseType, out var baseFloorPrefab))
            {
                baseFloor = Instantiate(baseFloorPrefab);
            }
            else
            {
                throw new Exception($"Unknown base type {baseType}!");
            }

            return baseFloor;
        }

        private GameObject CreateSpire()
        {

            throw new NotImplementedException();
        }

        private GameObject CreateAdditionalElements()
        {

            throw new NotImplementedException();
        }
    }
    //TODO Check 
}
