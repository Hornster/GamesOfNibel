using System;
using System.Linq;
using Assets.Scripts.Game.Common.Data.Maps;
using Assets.Scripts.Game.Common.Data.ScriptableObjects.MapSelection;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Exceptions;

namespace Assets.Scripts.Game.Common.Helpers
{
    /// <summary>
    /// Maps one class into other.
    /// </summary>
    public class DataMapper
    {
        /// <summary>
        /// Maps mappable fields from map data scriptable object to raw map data one.
        /// </summary>
        /// <param name="source">Source of static information about the map.</param>
        /// <param name="basesRoot">Provides count of bases available in the scene.</param>
        /// <returns></returns>
        public RawMapData MapDataSOToRawMapData(MapDataSO source)
        {
            //BasesRoot cannot be put in MapDataSO since it's gameobject - SO cannot reference it.
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
                ThumbnailImgPath = source.ThumbnailImgPath,
                LilyBasesCount = source.LilyBasesCount,
                LotusBasesCount = source.LotusBasesCount,
                MultiTeamBasesCount = source.MultiTeamBasesCount,
                NeutralBasesCount = source.NeutralBasesCount
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

            mapData.BasesCount.Add(Teams.Lily, source.LilyBasesCount);
            mapData.BasesCount.Add(Teams.Lotus, source.LotusBasesCount);
            mapData.BasesCount.Add(Teams.Multi, source.MultiTeamBasesCount);
            mapData.BasesCount.Add(Teams.Neutral, source.NeutralBasesCount);

            return mapData;
        }
        
    }
}
