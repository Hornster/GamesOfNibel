using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common.Data.ScriptableObjects;
using Assets.Scripts.Common.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "MapDataSO", menuName = "ScriptableObjects/MapDataSO", order = 1)]
public class MapDataSO : ScriptableObject, IModJsonInfoFile
{
    public string ShownName;
    public string[] Authors;
    public GameplayModesEnum GameplayMode;
    public string Description;
    public SkillType[] RequiredSkills;

    [HideInInspector] 
    public string SceneBundlePath;
    /// <summary>
    /// Path on disk to the bundle containing preview images.
    /// </summary>
    [HideInInspector]
    public string PreviewBundlePath;
    /// <summary>
    /// Stores path to the preview image for the map.
    /// Inner field (is set during bundle asset building).
    /// </summary>
    [HideInInspector]
    public string PreviewImgPath;
    /// <summary>
    /// Stores path to the thumbnail image for the map.
    /// Inner field (is set during bundle asset building).
    /// </summary>
    [HideInInspector]
    public string ThumbnailImgPath;
    /// <summary>
    /// Property renders this value invisible to the inspector. It is used upon saving the map mod.
    /// Inner field (is set during bundle asset building).
    /// </summary>
    [HideInInspector]
    public string SceneId;
    /// <summary>
    /// Resets fields that are not shown to the user as these are used to create json file.
    /// </summary>
    public void ResetInnerFields()
    {
        PreviewImgPath = "";
        ThumbnailImgPath = "";
        SceneId = "";
        SceneBundlePath = "";
        PreviewBundlePath = "";

        //This is MY CODE and I'm going to put ANY MONSTROSITIES I WANT in here 'kay? 'kay.
    }

    public string GetJsonString()
    {
        return JsonUtility.ToJson(this);
    }
}