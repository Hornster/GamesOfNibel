using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Modding.MapCreation.Scripts
{
    public class MapModAssembler : EditorWindow
    {
        private string _mapName;
        private MapDataSO _mapDataSO;

        private SceneAsset _scene;
        private Sprite _previewImage;
        private Sprite _thumbnailImage;

        [MenuItem("Window/GoNMapModAssembler")]
        public static void ShowWindow()
        {
            GetWindow<MapModAssembler>("Map Mod Assembler");
        }
        /// <summary>
        /// Checks if scene, preview image and thumbnail are saved on disk.
        /// </summary>
        /// <param name="unityObject">Object to check if it is on disk as an asset or located in the scene.</param>
        /// <returns>(isSceneOnDisk, isPreviewImgOnDisk, isThumbnailOnDisk)</returns>
        private bool IsAllowedSceneObject(Object unityObject)
        {
            return !EditorUtility.IsPersistent(unityObject);
        }
        void OnGUI()
        {
            _mapName = EditorGUILayout.TextField("MapName");
            _mapDataSO = (MapDataSO) EditorGUILayout.ObjectField("Map data SO:", _mapDataSO, typeof(MapDataSO), IsAllowedSceneObject(_mapDataSO));
            _scene = (SceneAsset)EditorGUILayout.ObjectField("Map scene: ", _scene, typeof(SceneAsset), IsAllowedSceneObject(_scene));
            _previewImage = (Sprite)EditorGUILayout.ObjectField("Map preview Image:", _previewImage, typeof(Sprite), IsAllowedSceneObject(_previewImage));
            _thumbnailImage = (Sprite)EditorGUILayout.ObjectField("Map thumbnail Image:", _thumbnailImage, typeof(Sprite), IsAllowedSceneObject(_thumbnailImage));

            if (GUILayout.Button("Create map mod"))
            {
                CreateMapMod();
            }
        }
        private string GetOutputPath()
        {
            return _mapDataSO.MapAssetBundleConstants.MapsFolderLocation + _scene.name;
        }

        private List<string> GetAssetsNames()
        {
            var names = new List<string>();
            names.Add(_scene.name);
            names.Add(_previewImage.name);
            names.Add(_thumbnailImage.name);

            return names;
        }

        private List<string> GetAssetsAddressables()
        {
            var bundleConstants = _mapDataSO.MapAssetBundleConstants;
            var addressables = new List<string>();
            addressables.Add(bundleConstants.SceneFolderName + _scene.name);
            addressables.Add(bundleConstants.PreviewImageFolderName + _previewImage.name);
            addressables.Add(bundleConstants.ThumbnailImageFolderName + _thumbnailImage.name);

            return addressables;
        }
        private void CreateMapMod()
        {
            var baseDir = GetOutputPath();
            var assetBundleBuilds = new List<AssetBundleBuild>();
            var assetBundleBuild = new AssetBundleBuild();

            assetBundleBuild.assetBundleName = _mapName;
            assetBundleBuild.assetNames = GetAssetsNames().ToArray();
            assetBundleBuild.addressableNames = GetAssetsAddressables().ToArray();

            assetBundleBuilds.Add(assetBundleBuild);

            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }

            BuildPipeline.BuildAssetBundles(baseDir, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }
        /// <summary>
        /// Writes down the data about the map into a JSON file that's located in the main
        /// directory of the map.
        /// </summary>
        /// <param name="baseDir">Base directory where the mod is being saved.</param>
        private void CreateJSONInfoFile(string baseDir)
        {
            var jsonMapData = JsonUtility.ToJson(_mapDataSO);
            baseDir += Path.PathSeparator + _mapName + ".json";
            File.WriteAllText(baseDir, jsonMapData);
        }

    }
}

//https://www.youtube.com/watch?v=1zROlULebXg - creating build pipeline for asset bundles
//https://www.turiyaware.com/blog/creating-a-moddable-unity-game - modding with unity
//https://www.youtube.com/watch?v=491TSNwXTIg - how to make editor window unity