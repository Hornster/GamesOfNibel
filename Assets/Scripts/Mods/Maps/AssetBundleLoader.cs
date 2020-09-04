using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.GUI.Menu.MapSelection;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Mods.Maps
{
    public class AssetBundleLoader : MonoBehaviour
    {
        [SerializeField] private MapDataAssetBundleConstants _mapModsConstants;
        /// <summary>
        /// Used to serialize raw map data.
        /// </summary>
        [SerializeField] private MapDataSO _defaultSO;
        /// <summary>
        /// Reads the maps directory. Returns available maps.
        /// </summary>
        public List<MapData> LoadMapAssetBundle()
        {
            var readPotentialMaps = SeekMapDirectory();
            var parsedMapsInfo = ParseJsonInfo(readPotentialMaps);
            var loadedMapData = LoadPreviewImagesBundle(parsedMapsInfo);

            return loadedMapData;
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
        private List<MapDataSO> ParseJsonInfo(List<string> potentialMaps)
        {
            var mapsInfos = new List<MapDataSO>(potentialMaps.Count);

            foreach (var potentialMap in potentialMaps)
            {
                var readMapAttempt = Instantiate(_defaultSO);
                JsonUtility.FromJsonOverwrite(potentialMap, readMapAttempt);
                //TODO: Add check if the map exists. Simply check if the file is present.
                if (readMapAttempt != null)
                {
                    mapsInfos.Add(readMapAttempt);
                }
            }

            return mapsInfos;
        }
        /// <summary>
        /// Loads scene bundle and reads data, including images, where available. 
        /// </summary>
        /// <param name="mapInfos">Raw info about images. Will be used to find and read from asset bundles.</param>
        private List<MapData> LoadPreviewImagesBundle(List<MapDataSO> mapInfos)
        {
            var loadedBundles = new List<MapData>();
            foreach (var mapInfo in mapInfos)
            {
                var newMapData = new MapData();
                var previewBundle = AssetBundle.LoadFromFile(mapInfo.PreviewBundlePath);

                var assetNames = previewBundle.GetAllAssetNames();
                foreach (var assetName in assetNames)
                {
                    Debug.Log(assetName);
                }
                
                if (previewBundle == null)
                {//If no asset bundle was found, no need to perform further operations for this file.
                    //TODO: report error about not found map.
                    continue;
                }

                newMapData.ReadData(mapInfo);
                if (previewBundle.Contains(mapInfo.PreviewImgPath))
                {
                    //TODO despite saying that there's an image, it cannot load the asset. Eh.
                    var previewImg = previewBundle.LoadAsset<Sprite>(mapInfo.PreviewImgPath);
                    newMapData.PreviewImg = previewImg;
                }

                if (previewBundle.Contains(mapInfo.ThumbnailImgPath))
                {
                    var thumbnailImg = previewBundle.LoadAsset<Sprite>(mapInfo.ThumbnailImgPath);
                    newMapData.ThumbnailImg = thumbnailImg;
                }

                loadedBundles.Add(newMapData);
            }

            return loadedBundles;
        }
        /// <summary>
        /// Loads the scene bundle on demand.
        /// </summary>
        public void LoadMapSceneBundle(MapData mapData)
        {
            //TODO
        }
    }
}

//https://forum.unity.com/threads/how-to-assetbundle-and-load-a-whole-scene.545904/ - loading a scene from an asset bundle