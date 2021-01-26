using Assets.Scripts.Common.Data.Maps;
using Assets.Scripts.Common.Enums;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Common.Data.ScriptableObjects.MapSelection
{
    /// <summary>
    /// Input point for map mod assembler - stores all required data about given map. EDITOR ONLY!!!
    /// </summary>
    [CreateAssetMenu(fileName = "MapDataSO", menuName = "ScriptableObjects/MapDataSO", order = 1)]
    public class MapDataSO : ScriptableObject
    {
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

        [Header("Optional References")] 
        [TextArea]
        public string Notes = "These references are not required but may speed up map assembling process when present.";
        [Tooltip("Reference to parent object that stores bases.")]
        public BasesRoot BasesRoot; 
        //public SceneAsset Scene;

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