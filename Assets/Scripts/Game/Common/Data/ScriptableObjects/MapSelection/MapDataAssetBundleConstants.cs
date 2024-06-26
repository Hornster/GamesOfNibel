﻿using System.IO;
using Assets.Scripts.Game.Common.Data.Constants;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Data.ScriptableObjects.MapSelection
{
    /// <summary>
    /// Stores information about the path elements of a map mod asset bundle.
    /// </summary>
    [CreateAssetMenu(fileName = MapDataAssenBundleConstantsName, 
        menuName = SGConstants.SGSOMenuPath + SGConstants.SGSOMapSelectionRelativePath + MapDataAssenBundleConstantsName, order = 1)]
    public class MapDataAssetBundleConstants : ScriptableObject
    {
        public const string MapDataAssenBundleConstantsName = "MapDataABConstants";
        /// <summary>
        /// The first part of the path in the asset bundle.
        /// </summary>
        [Header("Before sceneId")]
        public readonly string MapsFolderLocation = "Assets" + Path.DirectorySeparatorChar
                                                             + "Mods" + Path.DirectorySeparatorChar
                                                             + "AssetBundles" + Path.DirectorySeparatorChar 
                                                             + "Maps" + Path.DirectorySeparatorChar;

        [Header("Bundle name suffix")] 
        public readonly string BundleNameSuffix = "Map Mod";

        /// <summary>
        /// Constant path chunk added to the main map mod base directory designated for the preview images
        /// bundle to be stored in there.
        /// </summary>
        public readonly string PreviewImagesBundleFolder = "Preview";
        /// <summary>
        /// Suffix for the preview images bundle name for given map.
        /// </summary>
        public readonly string PreviewImagesBundleSuffix = "PreviewImages";
        /// <summary>
        /// The extension of the config file.
        /// </summary>
        public static readonly string MapConfigFileExtension = ".json";
    }
}
// The parts present here are constants that shall not be changed.
// Their usage order shall remain unchanged, too.
// -MapsFolderLocation always goes first.
// -Name of the scene goes next. This shall be as unique as possible (note that this is the name of the
// SCENE, not of the map. Name of the scene is visible to the application while name of the map is visible to players.).
// -Depending on the folder contents - one of fields under the [Header("After sceneId")].