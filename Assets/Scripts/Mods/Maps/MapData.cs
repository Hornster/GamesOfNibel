using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common.Data.ScriptableObjects.MapSelection;
using Assets.Scripts.Common.Enums;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Mods.Maps
{
    /// <summary>
    /// Stores data of a single loaded map.
    /// </summary>
    public class MapData
    {
        public string ShownMapName { get; set; }
        public List<string> Authors { get; set; }
        public Sprite PreviewImg { get; set; }
        public Sprite ThumbnailImg { get; set; }
        public GameplayModesEnum GameplayMode { get; set; }
        public string Description { get; set; }
        public List<SkillType> RequiredSkills { get; set; }
        public string ScenePath { get; set; }
        public string SceneBundlePath { get; set; }
    }
}
