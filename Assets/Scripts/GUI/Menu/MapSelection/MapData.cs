using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine.UI;

namespace Assets.Scripts.GUI.Menu.MapSelection
{
    /// <summary>
    /// Stores data of a single loaded map.
    /// </summary>
    public class MapData
    {
        public string ShownName { get; set; }
        public Image PreviewImg { get; set; }
        public Image ThumbnailImg { get; set; }
        public List<string> Authors { get; set; }
        public GameplayModesEnum GameplayMode { get; set; }
        public string Description { get; set; }
        public List<SkillType> RequiredSkills { get; set; }
        public string SceneId { get; set; }
    }
}
