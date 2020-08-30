using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "MapDataSO", menuName = "ScriptableObjects/MapDataSO", order = 1)]
public class MapDataSO : ScriptableObject
{
    public string ShownName;
    public string PreviewImgPath;
    public string ThumbnailImgPath;
    public string[] Authors;
    public GameplayModesEnum GameplayMode;
    public string Description;
    public SkillType[] RequiredSkills;
    public string SceneId;
    [NonSerialized]
    public MapDataAssetBundleConstants MapAssetBundleConstants;
}
