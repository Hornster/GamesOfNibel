using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Common.Data.ScriptableObjects.MapSelection;
using Assets.Scripts.Common.Exceptions;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.GUI.Menu.MapSelection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Mods.Maps
{
    public class MapAssetBundleLoader : MonoBehaviour
    {
        [SerializeField] private MapDataAssetBundleConstants _mapModsConstants;
        /// <summary>
        /// Used to serialize raw map data.
        /// </summary>
        [SerializeField] private MapDataSO _defaultSO;
        /// <summary>
        /// If part of info of the map is not found, this will be used to provide default values.
        /// </summary>
        [SerializeField] private MapSelectionDefaults _mapDefaults;
        /// <summary>
        /// Contains list of errors that emerged during last mod loading (searching for json files and reading mass data).
        /// Errors concerning single action (like selecting a map) are not contained in here and are simply thrown.
        /// </summary>
        public List<ModLoadingException> ErrorsList { get; private set; }
        /// <summary>
        /// Reads the maps directory. Returns available maps.
        /// </summary>
        public List<MapData> LoadMapAssetBundle()
        {
            ResetLastErrorsList();

            var readPotentialMaps = SeekMapDirectory();
            var parsedMapsInfo = ParseJsonInfo(readPotentialMaps);
            var loadedMapData = LoadPreviewImagesBundle(parsedMapsInfo);

            return loadedMapData;
        }
        /// <summary>
        /// Loads the scene bundle on demand. If managed to load it - returns the bundle.
        /// Returned bundle is already checked against the presence of the target map (scene) in it.
        /// If bundle cannot be loaded - throws exception of ModLoadingException type.
        /// </summary>
        public AssetBundle LoadMapSceneBundle(MapData mapData)
        {
            var loadedMapBundle = AssetBundle.LoadFromFile(mapData.SceneBundlePath);

            if (loadedMapBundle == null)
            {
                throw new ModLoadingException($"Unable to load map bundle at {mapData.SceneBundlePath}! Check names integrity and whether are there files missing.");
            }

            var availableScenes = loadedMapBundle.GetAllScenePaths();

            foreach (var availableScene in availableScenes)
            {
                if (availableScene.Equals(mapData.ScenePath))
                {
                    return loadedMapBundle;
                }
            }

            throw new ModLoadingException(
                $"Unable to load map scene at {mapData.ScenePath} in {mapData.SceneBundlePath} bundle! Check names integrity and whether are there files missing.");
            
        }
        /// <summary>
        /// Resets the errors  list.
        /// </summary>
        private void ResetLastErrorsList()
        {
            ErrorsList = new List<ModLoadingException>();
        }
        /// <summary>
        /// Seeks for maps (their json files) in the maps directory. Returns read found files as strings (one string per file).
        /// </summary>
        private List<string> SeekMapDirectory()
        {
            var appLocation = Path.GetFullPath(".");
            var mapModsPath = Path.Combine(appLocation, _mapModsConstants.MapsFolderLocation);
            var searchedFilesExtension = "*" + MapDataAssetBundleConstants.MapConfigFileExtension;
            var potentialMapsFiles = Directory.GetFiles(mapModsPath, searchedFilesExtension, SearchOption.AllDirectories);
            var potentialMapsInfo = new List<string>(potentialMapsFiles.Length);

            foreach (var potentialMapFile in potentialMapsFiles)
            {
                potentialMapsInfo.Add(File.ReadAllText(potentialMapFile));
            }

            return potentialMapsInfo;
        }
        /// <summary>
        /// Parses provided JSON files and returns map description class instances.
        /// </summary>
        /// <param name="potentialMaps">Read JSON files.</param>
        /// <returns></returns>
        private List<RawMapData> ParseJsonInfo(List<string> potentialMaps)
        {
            var mapsInfos = new List<RawMapData>(potentialMaps.Count);

            foreach (var potentialMap in potentialMaps)
            {
                //var readMapAttempt = Instantiate(_defaultSO);
                try
                {
                    var readMapAttempt = JsonUtility.FromJson<RawMapData>(potentialMap);
                    if (readMapAttempt != null)
                    {
                        mapsInfos.Add(readMapAttempt);
                    }
                }
                catch (Exception ex)
                {
                    ErrorsList.Add(new ModLoadingException($"Could not parse mod config file! Additional info: \n" + ex.Message));
                }
                //TODO: Add check if the map exists. Simply check if the file is present.

            }

            return mapsInfos;
        }
        /// <summary>
        /// Tries to load a preview sprite under provided address in provided asset bundle. If successful - returns
        /// the loaded sprite. If failed - returns default sprite.
        /// </summary>
        /// <param name="previewBundle"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private Sprite LoadSinglePreviewImage(AssetBundle previewBundle, string path)
        {
            if (previewBundle != null)
            {
                if (previewBundle.Contains(path))
                {
                    var previewImg = previewBundle.LoadAsset<Sprite>(path);
                    return previewImg;
                }
            }

            return _mapDefaults.DefaultMapPreview;
        }
        /// <summary>
        /// Loads scene bundle and reads data, including images, where available. 
        /// </summary>
        /// <param name="mapInfos">Raw info about images. Will be used to find and read from asset bundles.</param>
        private List<MapData> LoadPreviewImagesBundle(List<RawMapData> mapInfos)
        {
            var mapper = new DataMapper();
            var loadedBundles = new List<MapData>();
            foreach (var mapInfo in mapInfos)
            {
                var newMapData = mapper.MapDataFromRawMapData(mapInfo);
                var previewBundle = AssetBundle.LoadFromFile(mapInfo.PreviewBundlePath);

                newMapData.PreviewImg = LoadSinglePreviewImage(previewBundle, mapInfo.PreviewImgPath);
                newMapData.ThumbnailImg = LoadSinglePreviewImage(previewBundle, mapInfo.ThumbnailImgPath);

                loadedBundles.Add(newMapData);
            }

            return loadedBundles;
        }
    }
}

//https://forum.unity.com/threads/how-to-assetbundle-and-load-a-whole-scene.545904/ - loading a scene from an asset bundle