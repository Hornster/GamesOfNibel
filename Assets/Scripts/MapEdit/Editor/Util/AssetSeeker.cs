using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.MapEdit.Editor.Util
{
    /// <summary>
    /// Seeks out assets through AssetDatabase.
    /// </summary>
    /// <typeparam name="T">Type to look for.</typeparam>
    public class AssetSeeker<T> where T: ScriptableObject, new()
    {
        /// <summary>
        /// Seeks for given asset and returns it if found. Throws exception if nothing was found or if
        /// more than one file was found.
        /// </summary>
        /// <param name="pathToAsset">Path to where the search should be done.</param>
        /// <param name="assetName">Name of the asset we are looking for.</param>
        /// <returns></returns>
        public T FindBaseMarkerFactorySo(string pathToAsset, string assetName)
        {
            var hitsGUIDs = AssetDatabase.FindAssets(assetName, new[] { pathToAsset });

            if (hitsGUIDs.Length <= 0)
            {
                throw new Exception($"Cannot find {assetName} at {pathToAsset}!");
            }
            else if (hitsGUIDs.Length > 1)
            {
                throw new Exception($"Only one {assetName} is allowed at {pathToAsset}!");
            }

            var pathToFirstAsset = AssetDatabase.GUIDToAssetPath(hitsGUIDs[0]);
            return AssetDatabase.LoadAssetAtPath<T>(pathToFirstAsset);
        }

        /// <summary>
        /// Creates asset of type T under provided path with provided name.
        /// </summary>
        /// <param name="pathToAsset"></param>
        /// <param name="assetName"></param>
        /// <param name="assetExtension">Extension for the asset. Without the dot.</param>
        public T CreateAsset(string pathToAsset, string assetName, string assetExtension)
        {
            var objectToSave = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(objectToSave, pathToAsset + assetName + "." + assetExtension);

            return objectToSave;
        }
    }
}
