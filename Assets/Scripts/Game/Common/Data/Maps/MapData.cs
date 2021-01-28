using System.Collections.Generic;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Data.Maps
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
        public int MultiTeamBasesCount { get; set; }
        public int NeutralBasesCount { get; set; }
        public int LotusBasesCount { get; set; }
        public int LilyBasesCount { get; set; }
    }
}
