using UnityEditor;

namespace Assets.Editor.Modding.MapCreation.Scripts.Util
{
    /// <summary>
    /// Used to update the inspector to include freshly created through code bundles.
    /// </summary>
    public class AssetBundleReporter
    {
        /// <summary>
        /// Iterates through all asset names in the bundle and assigns matching asset to the bundle, so it can
        /// be reflected in the inspector. 
        /// </summary>
        /// <param name="assetNames">Names of assets that should be assigned to the bundle.</param>
        /// <param name="assetBundleName">Name of the bundle.</param>
        /// <param name="assetBundleVariant">Variant of the bundle.</param>
        public static void AssignAssetsToBundle(ref string[] assetNames, string assetBundleName, string assetBundleVariant)
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
        private static void AssignAssetToBundle(string path, string assetBundleName, string assetBundleVariant)
        {
            var assetImporter = AssetImporter.GetAtPath(path);

            assetImporter.SetAssetBundleNameAndVariant(assetBundleName, assetBundleVariant);
            assetImporter.SaveAndReimport();
        }
    
    }
}
