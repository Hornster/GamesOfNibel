using Assets.Scripts.Game.Common.Data.Maps;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Data.ScriptableObjects.MapSelection
{
    /// <summary>
    /// Input point for map mod assembler - stores all required data about given map. EDITOR ONLY!!!
    /// </summary>
    [CreateAssetMenu(fileName = MapDataSOName, 
        menuName = SGConstants.SGSOMenuPath + SGConstants.SGSOMapSelectionRelativePath + MapDataSOName, order = 1)]
    public class MapDataSO : ScriptableObject
    {
        public const string MapDataSOName = "MapDataSO";

        [Header("Map description")]
        public string ShownMapName;
        public string[] Authors;
        public GameplayModesEnum GameplayMode;
        public string Description;
        public SkillType[] RequiredSkills;

        [Header("Preview images")]
        public Sprite PreviewImg;
        public Sprite ThumbnailImg;
        /// <summary>
        /// Constants for bundle assets.
        /// </summary>
        [Header("Required references")]
        public MapDataAssetBundleConstants MapAssetBundleConstants;

       
        [HideInInspector] 
        public string SceneBundlePath;
        /// <summary>
        /// Path on disk to the bundle containing preview images.
        /// </summary>
        [HideInInspector]
        public string PreviewBundlePath;
        /// <summary>
        /// Stores path to the preview image for the map inside the asset bundle.
        /// Inner field (is set during bundle asset building).
        /// </summary>
        [HideInInspector]
        public string PreviewImgPath;
        /// <summary>
        /// Stores path to the thumbnail image for the map inside the asset bundle.
        /// Inner field (is set during bundle asset building).
        /// </summary>
        [HideInInspector]
        public string ThumbnailImgPath;
        /// <summary>
        /// Property renders this value invisible to the inspector. It is used upon saving the map mod.
        /// Inner field (is set during bundle asset building).
        /// </summary>
        [HideInInspector]
        public string ScenePath;
        /// <summary>
        /// How many universal bases (for all teams) does the map have.
        /// </summary>
        public int MultiTeamBasesCount;
        /// <summary>
        /// How many neutral bases does the map have.
        /// </summary>
        public int NeutralBasesCount;
        /// <summary>
        /// How many Lotus team bases does the map have.
        /// </summary>
        public int LotusBasesCount;
        /// <summary>
        /// How many Lotus team bases does the map have.
        /// </summary>
        public int LilyBasesCount;
        /// <summary>
        /// Resets fields that are not shown to the user as these are used to create json file.
        /// </summary>
        public void ResetInnerFields()
        {
            PreviewImgPath = "";
            ThumbnailImgPath = "";
            ScenePath = "";
            SceneBundlePath = "";
            PreviewBundlePath = "";

            //This is MY CODE and I'm going to put ANY MONSTROSITIES I WANT in here 'kay? 'kay.
        }

    }
}