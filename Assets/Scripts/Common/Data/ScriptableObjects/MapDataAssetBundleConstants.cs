using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Stores information about the path elements of a map mod asset bundle.
/// </summary>
[CreateAssetMenu(fileName = "MapDataABConstants", menuName = "ScriptableObjects/MapDataAssetBundleConstants", order = 1)]
public class MapDataAssetBundleConstants : ScriptableObject
{
    /// <summary>
    /// The first part of the path in the asset bundle.
    /// </summary>
    [Header("Before sceneId")]
    public readonly string MapsFolderLocation = "Assets" + Path.DirectorySeparatorChar 
                                                         + "AssetBundles" + Path.DirectorySeparatorChar 
                                                         + "Maps" + Path.DirectorySeparatorChar;

    [Header("SceneId placeholder")] 
    public readonly string ScenePlaceholder = "$SceneName$";

    [Header("After sceneId")]
    public readonly string PreviewImageFolderName = "PreviewImg" + Path.DirectorySeparatorChar;
    public readonly string ThumbnailImageFolderName = "ThumbnailImg" + Path.DirectorySeparatorChar;
    public readonly string SceneFolderName = "Scene" + Path.DirectorySeparatorChar;
}
// The parts present here are constants that shall not be changed.
// Their usage order shall remain unchanged, too.
// -MapsFolderLocation always goes first.
// -Name of the scene goes next. This shall be as unique as possible (note that this is the name of the
// SCENE, not of the map. Name of the scene is visible to the application while name of the map is visible to players.).
// -Depending on the folder contents - one of fields under the [Header("After sceneId")].