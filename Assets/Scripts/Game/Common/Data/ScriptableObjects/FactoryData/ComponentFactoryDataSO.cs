using Assets.Scripts.Game.Common.CustomCollections.DefaultCollectionsSerialization.Dictionary;
using Assets.Scripts.Game.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Data.ScriptableObjects.FactoryData
{
    /// <summary>
    /// Stores references to component prefabs for bases factory.
    /// </summary>
    [CreateAssetMenu(fileName = ComponentFactoryDataSOName,
        menuName = SGConstants.SGSOMenuPath + SGConstants.SGSOFactoryDataRelativePath + ComponentFactoryDataSOName, order = 1)]
    public class ComponentFactoryDataSO : ScriptableObject
    {
        public const string ComponentFactoryDataSOName = "ComponentFactoryDataSO";


        [Header("Base marker main object")]
        [Tooltip("The main base marker object which contains the components and universal scripts.")]
        [SerializeField]
        private GameObject _baseMainPrefab;
        [Header("Base markers components")]
        [SerializeField]
        private BaseTypeGameObjectDictionary _baseFloorComponents = new BaseTypeGameObjectDictionary();
        [SerializeField]
        private BaseTypeGameObjectDictionary _baseSpireComponents = new BaseTypeGameObjectDictionary();
        [Tooltip("Additional components that can be assigned to given base, like spawn areas.")]
        [SerializeField]
        private BaseTypeGameObjectDictionary _baseAdditionalComponents = new BaseTypeGameObjectDictionary();

        public GameObject BaseMainPrefab => _baseMainPrefab;
        public BaseTypeGameObjectDictionary BaseFloorComponents => _baseFloorComponents;
        public BaseTypeGameObjectDictionary BaseSpireComponents => _baseSpireComponents;
        public BaseTypeGameObjectDictionary BaseAdditionalComponents => _baseAdditionalComponents;



        public GameObject CreateBaseFloor(BaseTypeEnum baseType)
        {
            GameObject baseFloor = null;
            var baseFloorDict = BaseFloorComponents.dictionary;
            if (baseFloorDict.TryGetValue(baseType, out var baseFloorPrefab))
            {
                baseFloor = Instantiate(baseFloorPrefab);
            }

            return baseFloor;
        }

        public GameObject CreateSpire(BaseTypeEnum baseType)
        {
            GameObject baseSpire = null;
            var baseSpireDict = BaseSpireComponents.dictionary;
            if (baseSpireDict.TryGetValue(baseType, out var baseSpirePrefab))
            {
                baseSpire = Instantiate(baseSpirePrefab);
            }

            return baseSpire;
        }

        public GameObject CreateAdditionalElements(BaseTypeEnum baseType)
        {
            GameObject baseAdditionalElements = null;
            var baseAdditionalElementsDict = BaseSpireComponents.dictionary;
            if (baseAdditionalElementsDict.TryGetValue(baseType, out var baseAdditionalElementsPrefab))
            {
                baseAdditionalElements = Instantiate(baseAdditionalElementsPrefab);
            }

            return baseAdditionalElements;
        }
    }
}
