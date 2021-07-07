using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.MapEdit.Editor.Data.Constants;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.MapEdit.Editor.Util
{
    /// <summary>
    /// Seeks out assets through AssetDatabase.
    /// </summary>
    /// <typeparam name="T">Type to look for.</typeparam>
    public class AssetSeeker<T> where T: Object, new()
    {
        private string RemoveLastSlashFromPath(string path)
        {
            int lastCharacterIndex = path.Length - 1;

            if (path.Length <= 0)
            {
                return path;
            }

            if (path[lastCharacterIndex] == '/' || path[lastCharacterIndex] == '\\')
            {
                path = path.Remove(lastCharacterIndex);
            }

            return path;
        }

        private List<string> GUIDsToAssetPaths(string[] guids)
        {
            var paths = new List<string>(guids.Length);
            foreach (var guid in guids)
            {
                paths.Add(AssetDatabase.GUIDToAssetPath(guid));
            }

            return paths;
        }

        private List<string> FilterSOAssets(List<string> foundAssetsPaths)
        {
            var filteredAssetsPaths = new List<string>();
            foreach (var foundAssetPath in foundAssetsPaths)
            {
                var foundAssetExtension =
                    foundAssetPath.Substring(foundAssetPath.Length -
                                             SGMapEditConstants.ScriptableObjectsExtension.Length);
                if (foundAssetExtension.Equals(SGMapEditConstants.ScriptableObjectsExtension))
                {
                    filteredAssetsPaths.Add(foundAssetPath);
                }
            }

            return filteredAssetsPaths;
        }
        /// <summary>
        /// Seeks for given asset and returns it if found. Throws exception if nothing was found or if
        /// more than one file was found.
        /// </summary>
        /// <param name="pathToAsset">Path to where the search should be done.</param>
        /// <param name="assetName">Name of the asset we are looking for.</param>
        /// <returns></returns>
        public T FindAsset(string pathToAsset, string assetName)
        {
            pathToAsset = RemoveLastSlashFromPath(pathToAsset); //the path to asset cannot have a slash or a backslash. Otherwise Unity won't find the folder.
            var hitsGUIDs = AssetDatabase.FindAssets(assetName, new[] { pathToAsset });
            var paths = GUIDsToAssetPaths(hitsGUIDs);
            paths = FilterSOAssets(paths);

            if (paths.Count <= 0)
            {
                MapEditReporter.ReportWarning(string.Format(Errors.AssetSeekerAssetNotFoundAtPath, assetName, pathToAsset));
                return FindAsset(assetName);
            }
            else if (paths.Count > 1)
            {
                throw new Exception(string.Format(Errors.AssetSeekerMultipleAssetsFoundAtPath, assetName, pathToAsset));
            }

            var pathToFirstAsset = paths[0];
            return AssetDatabase.LoadAssetAtPath<T>(pathToFirstAsset);
        }

        public T FindAsset(string assetName)
        {
            var hitsGUIDs = AssetDatabase.FindAssets(assetName);
            var paths = GUIDsToAssetPaths(hitsGUIDs);
            paths = FilterSOAssets(paths);  //FindAssets return assets of given name, disregarding the extension.
                                            //We seek for the scriptable objects only, which have .asset extension.

            if (paths.Count <= 0)
            {
                throw new Exception(string.Format(Errors.AssetSeekerAssetNotFoundGlobally, assetName));
            }
            else if (paths.Count > 1)
            {
                throw new Exception(string.Format(Errors.AssetSeekerMultipleAssetsFoundGlobally, assetName));
            }

            var pathToFirstAsset = paths[0];
            return AssetDatabase.LoadAssetAtPath<T>(pathToFirstAsset);
        }

        /// <summary>
        /// Creates asset of type T under provided path with provided name.
        /// </summary>
        /// <param name="pathToAsset"></param>
        /// <param name="assetName"></param>
        /// <param name="assetExtension">Extension for the asset. Without the dot.</param>
        public U CreateScriptableObjectAsset<U>(string pathToAsset, string assetName, string assetExtension) where U: ScriptableObject, T
        {
            Debug.Log($"Creating new {assetName} asset.");
            var objectToSave = ScriptableObject.CreateInstance<U>();
            AssetDatabase.CreateAsset(objectToSave, pathToAsset + assetName + "." + assetExtension);

            return objectToSave;
        }
    }
}
