using System.Collections.Generic;
using System.IO;
using Assets.Editor.Scripts.Modding.MapCreation.Scripts.Util;
using Assets.Scripts.Game.Common.Data.Maps;
using Assets.Scripts.Game.Common.Data.ScriptableObjects.MapSelection;
using Assets.Scripts.Game.Common.Helpers;
using Assets.Scripts.MapEdit.Editor.Data;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Assets.Editor.Scripts.Modding.MapCreation.Scripts
{
    public class MapModAssembler : EditorWindow
    {
        //private string _mapName;
        private MapDataSO _mapDataSO;

        private SceneAsset _scene;


        /// <summary>
        /// Were all requirements met upon trying to create the map mod? Updated
        /// every time OnGUI is called.
        /// </summary>
        private bool _allRequirementsMet = true;

        [MenuItem(SGMapEditPaths.WindowsPath + "/Map Mod Assembler")]
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
                GUILayout.Label("A reference to the MapDataScriptableObject (MapDataSO) has to be provided.");
            }

            if (_mapDataSO?.ShownMapName?.Length <= 0)
            {
                _allRequirementsMet = false;
                GUILayout.Label("Your map must have a name.");
            }

            if (_mapDataSO?.MapAssetBundleConstants == null)
            {
                _allRequirementsMet = false;
                GUILayout.Label("A reference to the MapDataAssetBundleConstants has to be provided. It contains additional map info.");
            }
        }
        void OnGUI()
        {
            EditorGUILayout.HelpBox("MapDataSO contains description of the map. Can be found by right clicking in the project inspector, selecting Create->Scriptable Objects->MapDataSO", MessageType.Info);
            _mapDataSO = (MapDataSO) EditorGUILayout.ObjectField("Map data SO:", _mapDataSO, typeof(MapDataSO), IsAllowedSceneObject(_mapDataSO));
            _scene = (SceneAsset)EditorGUILayout.ObjectField("Scene file (asset):", _scene, typeof(SceneAsset), IsAllowedSceneObject(_scene));
            

            ChkInputData();


            if (GUILayout.Button("Create map mod"))
            {
                if (_allRequirementsMet)
                {
                    _mapDataSO.ScenePath = _scene.name;
                    CreateMapMod();
                }
            }
        }
        /// <summary>
        /// Creates and returns an output path for the asset bundle.
        /// </summary>
        /// <returns></returns>
        private string GetOutputPath()
        {
            return _mapDataSO.MapAssetBundleConstants.MapsFolderLocation + _mapDataSO.ShownMapName + Path.DirectorySeparatorChar + _scene.name;
        }
        /// <summary>
        /// Gets the name, together with relative path, of the provided asset.
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        private string GetAssetName(Object asset)
        {
            return AssetDatabase.GetAssetOrScenePath(asset);
        }
        /// <summary>
        /// Returns a list containing the names of the preview and thumbnail images for the bundle, together with relative paths.
        /// If no images were provided - the list is empty.
        /// </summary>
        /// <param name="mapDataSo">Map data object. If there are any preview images available for this map, their
        /// paths will be assigned to this object on the run.</param>
        /// <returns></returns>
        private List<string> GetMapImagesNames(MapDataSO mapDataSo)
        {
            var names = new List<string>();

            if (_mapDataSO.PreviewImg != null)
            {
                var previewImgName = GetAssetName(_mapDataSO.PreviewImg);
                names.Add(previewImgName);
                mapDataSo.PreviewImgPath = previewImgName;
            }

            if (_mapDataSO.ThumbnailImg != null)
            {
                var thumbnailImgName = GetAssetName(_mapDataSO.ThumbnailImg);
                names.Add(thumbnailImgName);
                mapDataSo.ThumbnailImgPath = thumbnailImgName;
            }

            return names;
        }
        /// <summary>
        /// Returns a list of addressables for preview and thumbnail images that shall be included in the bundle.
        /// If neither were provided - returns an empty list.
        /// </summary>
        /// <returns></returns>
        private List<string> GetPreviewImagesAddressables(MapDataSO mapDataSo)
        {
            var addressables = new List<string>();
             
            if (_mapDataSO.PreviewImg != null)
            {
                var previewImgName = GetAssetName(_mapDataSO.PreviewImg);
                addressables.Add(previewImgName);
            }

            if (_mapDataSO.ThumbnailImg != null)
            {
                var thumbnailImgName = GetAssetName(_mapDataSO.ThumbnailImg);
                addressables.Add(thumbnailImgName);
            }

            return addressables;
        }
        /// <summary>
        /// Manages creation of the preview images asset bundle for the map mod.
        /// </summary>
        /// <param name="assetBundleCreator">Bundle creator instance.</param>
        /// <param name="baseDir">Base direction for the bundle to be saved in.</param>
        private void CreatePreviewImagesBundle(AssetBundleCreator assetBundleCreator, string baseDir)
        {
            var assetBundleName = _mapDataSO.ShownMapName + _mapDataSO.MapAssetBundleConstants.PreviewImagesBundleSuffix;
            //Set new asset bundle.
            assetBundleCreator.CreateNewBundle(assetBundleName);

            //Set the names of the preview images...
            var assetBundleNames = GetMapImagesNames(_mapDataSO);
            if (assetBundleNames.Count <= 0)
            {
                return; //No need for creation of images preview if none are existent.
            }
            assetBundleCreator.SetAssetNames(assetBundleNames);
            
            //...the addressables...
            var assetAddressables = GetPreviewImagesAddressables(_mapDataSO);
            assetBundleCreator.SetAddressableNames(assetAddressables);

            //...and save the bundle with the preview images.
            var previewBaseDir = Path.Combine(baseDir + _mapDataSO.MapAssetBundleConstants.PreviewImagesBundleFolder);
            assetBundleCreator.SaveBundle(previewBaseDir);
            _mapDataSO.PreviewBundlePath = Path.Combine(previewBaseDir, assetBundleName);
        }
        /// <summary>
        /// Sequence of instructions for creating a single scene bundle.
        /// </summary>
        /// <param name="assetBundleCreator"></param>
        /// <param name="baseDir"></param>
        private void CreateSceneBundle(AssetBundleCreator assetBundleCreator, string baseDir)
        {
            var assetBundleNames = new List<string>();
            var assetAddressables = new List<string>();
            var sceneBundleName = _mapDataSO.ShownMapName + _mapDataSO.MapAssetBundleConstants.BundleNameSuffix;

            //Set new asset bundle.
            assetBundleCreator.CreateNewBundle(sceneBundleName);

            //Set the name of the scene...
            var sceneName = GetAssetName(_scene);
            _mapDataSO.ScenePath = sceneName;
            assetBundleNames.Add(sceneName);
            assetBundleCreator.SetAssetNames(assetBundleNames);

            //...the addressable...
            assetAddressables.Add(sceneName);
            assetBundleCreator.SetAddressableNames(assetAddressables);

            //...and save the bundle for the scene alone.
            assetBundleCreator.SaveBundle(baseDir);
            _mapDataSO.SceneBundlePath = Path.Combine(baseDir, sceneBundleName);
        }
        
        /// <summary>
        /// Manages the creation of the map mod - preview images and the scene itself.
        /// </summary>
        private void CreateMapMod()
        {
            //Clear fields that are not set in the inspector. These fields are
            //set here, to create JSON file that describes the bundle.
            _mapDataSO.ResetInnerFields();

            //First, setup the bundle that contains the preview and thumbnail images of the map.
            _mapDataSO.ShownMapName = StringManipulator.RemoveSpecialCharacters(_mapDataSO.ShownMapName);

            var assetBundleCreator = new AssetBundleCreator();
            var baseDir = GetOutputPath();
            
            CreatePreviewImagesBundle(assetBundleCreator, baseDir);
            CreateSceneBundle(assetBundleCreator, baseDir);

            var mapper = new DataMapper();
            var serializableMapData = mapper.MapDataSOToRawMapData(_mapDataSO);


            //TODO: Check if MapDataSO contains BasesRoot reference. If yes - use it to swiftly retrieve bases quantity.
            //TODO: If not - get all object from the scene and search for the object in there. https://docs.unity3d.com/ScriptReference/SceneManagement.Scene.GetRootGameObjects.html

            //var foundBases;

            assetBundleCreator.CreateJSONInfoFile(serializableMapData, baseDir, serializableMapData.ShownMapName);
        }

    }
}
//TODO: assets and scenes cannot be in the same bundle for some reason. First create bundle with images, then create the scene. DONE
//TODO: Test above thing.
//https://www.youtube.com/watch?v=1zROlULebXg - creating build pipeline for asset bundles
//https://www.turiyaware.com/blog/creating-a-moddable-unity-game - modding with unity
//https://www.youtube.com/watch?v=491TSNwXTIg - how to make editor window unity
//https://answers.unity.com/questions/1206997/mark-asset-as-assetbundle-from-script.html - how to mark asset as bundle