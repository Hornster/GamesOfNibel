using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Mods.Maps;
using UnityEngine;

namespace Assets.Scripts.GUI.Menu.MapSelection
{
    /// <summary>
    /// Main class for the map selection menu.
    /// </summary>
    public class MapSelectionMenuController : MonoBehaviour
    {
        [SerializeField] private MapLoader _mapLoader;
        [SerializeField] private PreviewManager _previewManager;

        /// <summary>
        /// The currently selected map. It will be launched when the player accepts the settings.
        /// </summary>
        private CustomSelectableControl _selectedMap;
        /// <summary>
        /// The map that is currently being previewed. It's just a preview, it won't be launched.
        /// </summary>
        private CustomSelectableControl _previewedMap;
        private void Awake()
        {
            var foundMaps = _mapLoader.LoadAvailableMaps();

            RegisterEventHandlers(foundMaps);
        }
        /// <summary>
        /// Registers all event handlers for map selectable controls.
        /// </summary>
        /// <param name="mapControls">Controls which need to have handlers registered.</param>
        private void RegisterEventHandlers(List<CustomSelectableControl> mapControls)
        {
            foreach (var mapControl in mapControls)
            {
                mapControl.RegisterOnPointedAt(MapPointedAt);
                mapControl.RegisterOnSelected(MapSelected);
                mapControl.RegisterOnStoppedPointingAt(StoppedPointingAtMap);
            }
        }
        /// <summary>
        /// Called when given map is being pointed at.
        /// </summary>
        /// <param name="mapControl">Currently pointed at control.</param>
        private void MapPointedAt(CustomSelectableControl mapControl)
        {
            _previewedMap = mapControl;
            _previewManager.UpdatePreview(mapControl.MapData);
        }
        /// <summary>
        /// Called when given map has been selected as match one.
        /// </summary>
        /// <param name="mapControl">Currently selected control.</param>
        private void MapSelected(CustomSelectableControl mapControl)
        {
            _selectedMap?.DisableSelection();
            _selectedMap = mapControl;
        }
        /// <summary>
        /// Called when player stopped pointing at given map.
        /// </summary>
        private void StoppedPointingAtMap()
        {
            //TODO
        }
    }
}
//TODO - changing the details of the map - new monobehavior on the way