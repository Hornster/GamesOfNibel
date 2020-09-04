using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.Menu.MapSelection
{
    /// <summary>
    /// Stores data of a single loaded map.
    /// </summary>
    public class MapData
    {
        public string ShownName { get; set; }
        public List<string> Authors { get; set; }
        public Sprite PreviewImg { get; set; }
        public Sprite ThumbnailImg { get; set; }
        public GameplayModesEnum GameplayMode { get; set; }
        public string Description { get; set; }
        public List<SkillType> RequiredSkills { get; set; }
        public string ScenePath { get; set; }
        public string SceneBundlePath { get; set; }

        public void ReadData(MapDataSO mapData)
        {
            ShownName = mapData.ShownName;
            Authors = mapData.Authors.ToList();
            GameplayMode = mapData.GameplayMode;
            Description = mapData.Description;
            RequiredSkills = mapData.RequiredSkills.ToList();
            ScenePath = mapData.ScenePath;
            SceneBundlePath = mapData.SceneBundlePath;
        }
    }
}
