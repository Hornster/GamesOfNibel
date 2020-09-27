using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Data.ScriptableObjects.MapSelection;
using Assets.Scripts.GUI.Menu;
using Assets.Scripts.GUI.Menu.MapSelection;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Mods.Maps
{
    /// <summary>
    /// Responsible for managing loaded maps.
    /// </summary>
    public class MapLoader : MonoBehaviour
    {
        /// <summary>
        /// Used to retrieve all of the available maps from the mods folder.
        /// </summary>
        [SerializeField]
        private MapAssetBundleLoader _mapBundleLoader;
        /// <summary>
        /// Prefab used to generate found map miniatures in selection window.
        /// </summary>
        [SerializeField]
        private GameObject _mapUIElementPrefab;
        /// <summary>
        /// Transform which will be the parent of all read maps.
        /// </summary>
        [SerializeField] private Transform _mapsContainer;

        private static AssetBundle _loadedMap;

        /// <summary>
        /// Loads available maps and returns references to their data.
        /// </summary>
        /// <returns></returns>
        public List<CustomSelectableControl> LoadAvailableMaps()
        {
            var availableMaps = _mapBundleLoader.LoadMapAssetBundle();
            var selectableControls = new List<CustomSelectableControl>();

            foreach (var map in availableMaps)
            {
                var mapIcon = Instantiate(_mapUIElementPrefab, Vector3.zero, Quaternion.identity, _mapsContainer);
                var mapDataAdapter = mapIcon.GetComponent<MapDataGameObjectAdapter>();
                selectableControls.Add(mapIcon.GetComponentInChildren<CustomSelectableControl>());
                mapDataAdapter.SetMapData(map);
            }

            return selectableControls;
        }
        /// <summary>
        /// Tries to load a single scene bundle under provided path. If not managed to load, throws exception of MapLoadingException type.
        /// </summary>
        /// <param name="mapData">Map which shall be loaded.</param>
        /// <returns></returns>
        public void TryLoadingMapBundle(MapData mapData)
        {
            _loadedMap = _mapBundleLoader.LoadMapSceneBundle(mapData);
        }
        /// <summary>
        /// Unloads all assets that contain map data.
        /// </summary>
        public void UnloadAvailableMaps()
        {
            _mapBundleLoader.UnloadLoadedAssetBundles();
        }
        /// <summary>
        /// Unloads the asset of the last played map (scene).
        /// </summary>
        public void UnloadLastLoadedMap()
        {
            if (_loadedMap != null)
            {
                _loadedMap.Unload(true);
                _loadedMap = null;
            }
        }
    }
}
