using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using Assets.Scripts.GUI.Menu.MapSelection;
using UnityEngine;
using UnityEngine.UI;
using UnityScript.Steps;

namespace Assets.Scripts.AssetBundles
{
    public class AssetBundleLoader : MonoBehaviour
    {
        private readonly string _mapsDirectoryPath = Path.DirectorySeparatorChar + "Assets" + Path.DirectorySeparatorChar
                                                     + "Mods" + Path.DirectorySeparatorChar
                                                     + "Maps";
        /// <summary>
        /// Reads the maps directory. Returns available maps.
        /// </summary>
        public void LoadMapAssetBundle()
        {
            var readPotentialMaps = SeekMapDirectory();
            var parsedMapsInfo = ParseJsonInfo(readPotentialMaps);
            var loadedMapData = LoadPreviewImagesBundle(parsedMapsInfo);
        }
        /// <summary>
        /// Seeks for maps (their json files) in the maps directory. Returns read found files as strings (one string per file).
        /// </summary>
        private List<string> SeekMapDirectory()
        {
            var potentialMapsFiles = Directory.GetFiles(_mapsDirectoryPath, "*.json");
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
                var readMapAttempt = (MapDataSO)JsonUtility.FromJson(potentialMap, typeof(MapDataSO));
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

                if (previewBundle == null)
                {//If no asset bundle was found, no need to perform further operations for this file.
                    //TODO: report error about not found map.
                    continue;
                }

                newMapData.ReadData(mapInfo);
                if (previewBundle.Contains(mapInfo.PreviewImgPath))
                {
                    var previewImg = previewBundle.LoadAsset<Image>(mapInfo.PreviewImgPath);
                    newMapData.PreviewImg = previewImg;
                }

                if (previewBundle.Contains(mapInfo.ThumbnailImgPath))
                {
                    var thumbnailImg = previewBundle.LoadAsset<Image>(mapInfo.ThumbnailImgPath);
                    newMapData.ThumbnailImg = thumbnailImg;
                }

                loadedBundles.Add(newMapData);
            }

            return loadedBundles;
        }
        /// <summary>
        /// Loads scene bundle on demand.
        /// </summary>
        public void LoadMapSceneBundle()
        {
            //TODO
        }
    }
}
