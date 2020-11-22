using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.CustomEvents;
using Assets.Scripts.Common.Data.NoDestroyOnLoad;
using Assets.Scripts.Common.Data.ScriptableObjects.ScenePassData;
using Assets.Scripts.Common.Exceptions;
using Assets.Scripts.Mods.Maps;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GUI.Menu.MapSelection
{
    /// <summary>
    /// Main class for the map selection menu.
    /// </summary>
    public class MapSelectionMenuController : MonoBehaviour
    {
        [Tooltip("SO that stores path to the dynamic objects loading scene. This scene is loaded" +
                 "before any map scene and is used to initialize dynamic data for the given" +
                 "map scene.")]
        [SerializeField] private LoadingScenePathSO _loadingScenePathSO;
        [SerializeField] private MapLoader _mapLoader;
        [SerializeField] private PreviewManager _previewManager;

        [Tooltip("Can be used to perform actions when the previewed details of the map have been changed.")]
        [SerializeField] private UnityEvent _onMapPointedAtChanged;
        [Tooltip("Called when the user confirmed selected map and its loading sequence has been started.")]
        [SerializeField] private StringUnityEvent _onMapLaunching;
        /// <summary>
        /// Will be called when something went wrong during selected map launching.
        /// </summary>
        [Tooltip("If something goes wrong with map launching, these handlers will be called.")]
        [SerializeField] private UnityEvent _onMapLoadFailure;
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

            _mapLoader.UnloadLastLoadedMap();
            
            RegisterEventHandlers(foundMaps);
        }
        /// <summary>
        /// Begins loading of the selected map.
        /// </summary>
        public void LaunchSelectedMap()
        {
            if (_selectedMap != null)
            {
                try
                {
                    var mapData = _selectedMap.MapData;
                    _mapLoader.TryLoadingMapBundle(mapData);
                    _mapLoader.UnloadAvailableMaps();

                    MatchData.GetInstance().SceneToLoad = mapData.ScenePath;

                    //perform transitions and launch the map.
                    _onMapLaunching?.Invoke(_loadingScenePathSO.LoadingScenePath);
                    //_onMapLaunching?.Invoke(mapData.ScenePath);
                }
                catch (ModLoadingException ex)
                {
                    Debug.LogError(ex);
                    _onMapLoadFailure?.Invoke();
                }
            }
            else
            {
                //Trying to load a map when none selected should not make any effects, INCLUDING transitions.
                _onMapLoadFailure?.Invoke();
            }
           
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
            _onMapPointedAtChanged?.Invoke();
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
        /// Called when player stopped pointing at any map.
        /// </summary>
        private void StoppedPointingAtMap()
        {
            if (_selectedMap != null)
            {
                _previewManager.UpdatePreview(_selectedMap.MapData);
                Debug.Log("Changed preview - stopped pointing at.");
            }
        }
    }
}
//TODO - changing the details of the map - new monobehavior on the way