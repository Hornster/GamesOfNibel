using UnityEngine;

namespace Assets.Scripts.Common.Data.ScriptableObjects.MapSelection
{
    /// <summary>
    /// Provides default values for map selection details.
    /// </summary>
    [CreateAssetMenu(fileName = "MapSelectionDefaults", menuName = "ScriptableObjects/MapSelectionDefaults", order = 1)]
    public class MapSelectionDefaults : ScriptableObject
    {
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
