using Assets.Scripts.Game.Common.Data.Constants;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Data.ScriptableObjects.MapSelection
{
    /// <summary>
    /// Provides default values for map selection details.
    /// </summary>
    [CreateAssetMenu(fileName = MapSelectionDefaultsName, 
        menuName = SGConstants.SGSOMenuPath + SGConstants.SGSOMapSelectionRelativePath + MapSelectionDefaultsName, order = 1)]
    public class MapSelectionDefaults : ScriptableObject
    {
        public const string MapSelectionDefaultsName = "MapSelectionDefaults";

        [Header("Description defaults")]
        public string DefaultMapName;
        public string DefaultMapAuthors;
        public string DefaultMapMode;
        public string DefaultMapDescription;

        [Header("Preview images defaults")]
        public Sprite DefaultMapPreview;
        public Sprite DefaultThumbnail;
    }
}
