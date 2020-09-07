using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
