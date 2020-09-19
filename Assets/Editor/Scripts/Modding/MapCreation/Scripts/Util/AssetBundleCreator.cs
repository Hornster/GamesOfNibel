using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Common.Data.ScriptableObjects.MapSelection;
using UnityEditor;

namespace Assets.Editor.Scripts.Modding.MapCreation.Scripts.Util
{
    /// <summary>
    /// Used to update the inspector to include freshly created through code bundles.
    /// </summary>
    public class AssetBundleCreator
    {
        /// <summary>
        /// Currently generated asset bundle.
        /// </summary>
        private AssetBundleBuild _currentAssetBundle;
        /// <summary>
        /// Creates just the instance of AssetBundleCreator. Still requires calling the
        /// CreateNewBundle method.
        /// </summary>
        public AssetBundleCreator()
        {

        }
        /// <summary>
        /// Creates new asset bundle with name and variant.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="variant"></param>
        public AssetBundleCreator(string name, string variant = "")
        {
            CreateNewBundle(name, variant);
        }
        /// <summary>
        /// Creates new asset bundle.
        /// </summary>
        /// <param name="name">Name of the new bundle.</param>
        /// <param name="variant">Variant of new bundle.</param>
        /// <returns></returns>
        public void CreateNewBundle(string name, string variant = "")
        {
            _currentAssetBundle = new AssetBundleBuild();
            _currentAssetBundle.assetBundleName = name;
            _currentAssetBundle.assetBundleVariant = variant;
        }
        /// <summary>
        /// Sets (overwrites) the names of the assets in the bundle to provided list.
        /// </summary>
        /// <param name="assetNames">List of names of the assets.</param>
        public void SetAssetNames(List<string> assetNames)
        {
            _currentAssetBundle.assetNames = assetNames.ToArray();
        }
        /// <summary>
        /// Add provided asset names to already existing ones.
        /// </summary>
        /// <param name="assetNames">Additional names that will be added.</param>
        public void AddAssetNames(List<string> assetNames)
        {
            var existingAssetNames = _currentAssetBundle.assetNames;
            var allNames = new List<string>(existingAssetNames.Length + assetNames.Count);
            
            foreach (var existingName in existingAssetNames)
            {
                allNames.Add(existingName);
            }

            foreach (var newName in assetNames)
            {
                allNames.Add(newName);
            }

            _currentAssetBundle.assetNames = allNames.ToArray();
        }
        /// <summary>
        /// Sets (overwrites) the addressables list for the bundle.
        /// </summary>
        /// <param name="addressableNames"></param>
        public void SetAddressableNames(List<string> addressableNames)
        {
            _currentAssetBundle.addressableNames = addressableNames.ToArray();
        }
        /// <summary>
        /// Adds new addressables to the bundle.
        /// </summary>
        /// <param name="assetAddressables"></param>
        public void AddAddressableNames(List<string> assetAddressables)
        {
            var existingAddressables = _currentAssetBundle.addressableNames;
            var allAddressables = new List<string>(assetAddressables.Count + existingAddressables.Length);

            foreach (var existingAddressable in existingAddressables)
            {
                allAddressables.Add(existingAddressable);
            }

            foreach (var newAddressable in assetAddressables)
            {
                allAddressables.Add(newAddressable);
            }

            _currentAssetBundle.addressableNames = allAddressables.ToArray();
        }
        /// <summary>
        /// Creates the asset bundle and saves the info about it in form of a json file.
        /// </summary>
        /// <param name="directory">Directory where the file should end up.</param>
        public void SaveBundle(string directory)
        {
            var assetBundleBuilds = new List<AssetBundleBuild>();
            assetBundleBuilds.Add(_currentAssetBundle);

            ChkIfDirExists(directory);

            //CreateJSONInfoFile(infoFile, directory, jsonFileName);
            BuildPipeline.BuildAssetBundles(directory, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

            AssignAssetsToBundle(ref _currentAssetBundle.assetNames, _currentAssetBundle.assetBundleName, _currentAssetBundle.assetBundleVariant);
        }
        /// <summary>
        /// Creates a json file that describes the mod at provided directory.
        /// </summary>
        /// <param name="infoFile">Information about the mod.</param>
        /// <param name="baseDir">Directory where the file should end up.</param>
        /// <param name="jsonFileName">Name of the json info file.</param>
        public void CreateJSONInfoFile(IModJsonInfoFile infoFile, string baseDir, string jsonFileName)
        {
            var jsonMapData = infoFile.GetJsonString();
            var jsonPath = baseDir + Path.DirectorySeparatorChar + jsonFileName + MapDataAssetBundleConstants.MapConfigFileExtension;

            ChkIfDirExists(baseDir);

            File.WriteAllText(jsonPath, jsonMapData);
        }
        /// <summary>
        /// Iterates through all asset names in the bundle and assigns matching asset to the bundle, so it can
        /// be reflected in the inspector. 
        /// </summary>
        /// <param name="assetNames">Names of assets that should be assigned to the bundle.</param>
        /// <param name="assetBundleName">Name of the bundle.</param>
        /// <param name="assetBundleVariant">Variant of the bundle.</param>
        private void AssignAssetsToBundle(ref string[] assetNames, string assetBundleName, string assetBundleVariant)
        {
            foreach (var assetName in assetNames)
            {
                AssignAssetToBundle(assetName, assetBundleName, assetBundleVariant);
            }
        }
        /// <summary>
        /// Assigns provided assets to bundle. This way the information will be reflected in the inspector.
        /// </summary>
        /// <param name="path">Relative path to the asset.</param>
        /// <param name="assetBundleName">The name of the asset bundle the asset will be assigned to.</param>
        /// <param name="assetBundleVariant">The variant of the asset bundle. Can be empty.</param>
        private void AssignAssetToBundle(string path, string assetBundleName, string assetBundleVariant)
        {
            var assetImporter = AssetImporter.GetAtPath(path);

            assetImporter.SetAssetBundleNameAndVariant(assetBundleName, assetBundleVariant);
            assetImporter.SaveAndReimport();
        }
        /// <summary>
        /// Checks if provided directory exists. If not - creates it.
        /// </summary>
        /// <param name="directory"></param>
        private void ChkIfDirExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
