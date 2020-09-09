using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Data.ScriptableObjects.MapSelection;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Mods.Maps
{
    /// <summary>
    /// Raw data of a map, ready for serialization or deserialization.
    /// </summary>
    public class RawMapData : IModJsonInfoFile
    {
        public string ShownMapName;
        public string[] Authors;
        public GameplayModesEnum GameplayMode;
        public string Description;
        public SkillType[] RequiredSkills;

        /// <summary>
        /// Path to the bundle with the scene on physical storage.
        /// </summary>
        public string SceneBundlePath;
        /// <summary>
        /// Path on disk to the bundle containing preview images.
        /// </summary>
        public string PreviewBundlePath;
        /// <summary>
        /// Stores path to the preview image for the map inside the asset bundle.
        /// Inner field (is set during bundle asset building).
        /// </summary>
        public string PreviewImgPath;
        /// <summary>
        /// Stores path to the thumbnail image for the map inside the asset bundle.
        /// Inner field (is set during bundle asset building).
        /// </summary>
        public string ThumbnailImgPath;
        /// <summary>
        /// Property renders this value invisible to the inspector. It is used upon saving the map mod.
        /// Inner field (is set during bundle asset building).
        /// </summary>
        public string ScenePath;

        public string GetJsonString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
