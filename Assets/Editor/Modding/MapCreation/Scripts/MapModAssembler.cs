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

        private MapDataAssetBundleConstants _mapAssetBundleConstants;

        private SceneAsset _scene;
        private Sprite _previewImage;
        private Sprite _thumbnailImage;

        /// <summary>
        /// Were all requirements met upon trying to create the map mod? Updated
        /// every time OnGUI is called.
        /// </summary>
        private bool _allRequirementsMet = true;

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
        /// <summary>
        /// Checks if required input data for the map is present.
        /// </summary>
        /// <returns></returns>
        private void ChkInputData()
        {
            _allRequirementsMet = true;
            if (_scene == null)
            {
                _allRequirementsMet = false;
                GUILayout.Label("A reference to the scene is required.");
            }
            if (_mapDataSO == null)
            {
                _allRequirementsMet = false;
                GUILayout.Label("A reference to the MapDataScriptableObject has to be provided. It contains additional map info.");
            }

            if (_mapAssetBundleConstants == null)
            {
                _allRequirementsMet = false;
                GUILayout.Label("A reference to the MapDataScriptableObject has to be provided. It contains additional map info.");
            }
            if (_mapName?.Length <= 0)
            {
                _allRequirementsMet = false;
                GUILayout.Label("Your map must have a name.");
            }
        }
        void OnGUI()
        {
            _mapName = EditorGUILayout.TextField("Map name", _mapName);
            _mapDataSO = (MapDataSO) EditorGUILayout.ObjectField("Map data SO:", _mapDataSO, typeof(MapDataSO), IsAllowedSceneObject(_mapDataSO));
            _mapAssetBundleConstants = (MapDataAssetBundleConstants) EditorGUILayout.ObjectField("Map bundle constants:", _mapAssetBundleConstants, typeof(MapDataAssetBundleConstants), IsAllowedSceneObject(_mapAssetBundleConstants));
            _scene = (SceneAsset)EditorGUILayout.ObjectField("Map scene: ", _scene, typeof(SceneAsset), IsAllowedSceneObject(_scene));
            _previewImage = (Sprite)EditorGUILayout.ObjectField("Map preview Image:", _previewImage, typeof(Sprite), IsAllowedSceneObject(_previewImage));
            _thumbnailImage = (Sprite)EditorGUILayout.ObjectField("Map thumbnail Image:", _thumbnailImage, typeof(Sprite), IsAllowedSceneObject(_thumbnailImage));
            
            ChkInputData();


            if (GUILayout.Button("Create map mod"))
            {
                if (_allRequirementsMet)
                {
                    _mapDataSO.SceneId = _scene.name;
                    CreateMapMod();
                }
            }
        }
        private string GetOutputPath()
        {
            return _mapAssetBundleConstants.MapsFolderLocation + _scene.name;
        }

        private string GetAssetName(Object asset)
        {
            return AssetDatabase.GetAssetOrScenePath(asset);
        }
        private List<string> GetAssetsNames()
        {
            var names = new List<string>();
            names.Add(GetAssetName(_scene));
            if (_previewImage != null)
            {
                names.Add(GetAssetName(_previewImage));
            }

            if (_thumbnailImage != null)
            {
                names.Add(GetAssetName(_thumbnailImage));
            }
            
            return names;
        }

        private List<string> GetAssetsAddressables()
        {
            var addressables = new List<string>();
            addressables.Add(_mapAssetBundleConstants.SceneFolderName + _scene.name);
            if (_previewImage != null)
            {
                addressables.Add(_mapAssetBundleConstants.PreviewImageFolderName + _previewImage.name);
            }

            if (_thumbnailImage != null)
            {
                addressables.Add(_mapAssetBundleConstants.ThumbnailImageFolderName + _thumbnailImage.name);
            }
            
            return addressables;
        }
        private void CreateMapMod()
        {
            var baseDir = GetOutputPath();
            var assetBundleBuilds = new List<AssetBundleBuild>();
            var assetBundleBuild = new AssetBundleBuild();

            assetBundleBuild.assetBundleName = _mapName + "Bundle";
            assetBundleBuild.assetNames = GetAssetsNames().ToArray();
            assetBundleBuild.addressableNames = GetAssetsAddressables().ToArray();

            assetBundleBuilds.Add(assetBundleBuild);

            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }

            CreateJSONInfoFile(baseDir);
            BuildPipeline.BuildAssetBundles(baseDir, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

        }
        /// <summary>
        /// Writes down the data about the map into a JSON file that's located in the main
        /// directory of the map.
        /// </summary>
        /// <param name="baseDir">Base directory where the mod is being saved.</param>
        private void CreateJSONInfoFile(string baseDir)
        {
            if (_scene != null)
            {
                _mapDataSO.SceneId = _scene.name;
            }

            var jsonMapData = JsonUtility.ToJson(_mapDataSO);
            var jsonPath = baseDir + ".json";
            File.WriteAllText(jsonPath, jsonMapData);
        }

    }
}

//https://www.youtube.com/watch?v=1zROlULebXg - creating build pipeline for asset bundles
//https://www.turiyaware.com/blog/creating-a-moddable-unity-game - modding with unity
//https://www.youtube.com/watch?v=491TSNwXTIg - how to make editor window unity