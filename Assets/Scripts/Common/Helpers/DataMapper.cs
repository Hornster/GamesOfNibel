using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Data.ScriptableObjects.MapSelection;
using Assets.Scripts.Mods.Maps;

namespace Assets.Scripts.Common.Helpers
{
    /// <summary>
    /// Maps one class into other.
    /// </summary>
    public class DataMapper
    {
        /// <summary>
        /// Maps mappable fields from map data scriptable object to raw map data one.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public RawMapData MapDataSOToRawMapData(MapDataSO source)
        {
            var rawData = new RawMapData()
            {
                Authors = source.Authors,
                Description = source.Description,
                GameplayMode = source.GameplayMode,
                PreviewImgPath = source.PreviewImgPath,
                PreviewBundlePath = source.PreviewBundlePath,
                RequiredSkills = source.RequiredSkills,
                SceneBundlePath = source.SceneBundlePath,
                ScenePath = source.ScenePath,
                ShownMapName = source.ShownMapName,
                ThumbnailImgPath = source.ThumbnailImgPath
            };

            return rawData;

        }
        /// <summary>
        /// Maps data from raw map data object to MapData.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public MapData MapDataFromRawMapData(RawMapData source)
        {
            var mapData = new MapData()
            {
                ShownMapName = source.ShownMapName,
                Authors = source.Authors.ToList(),
                GameplayMode = source.GameplayMode,
                Description = source.Description,
                RequiredSkills = source.RequiredSkills.ToList(),
                ScenePath = source.ScenePath,
                SceneBundlePath = source.SceneBundlePath
            };

            return mapData;
        }
    }
}
