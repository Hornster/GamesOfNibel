﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Data.Maps;
using Assets.Scripts.Common.Data.ScriptableObjects.MapSelection;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.Common.Exceptions;
using Assets.Scripts.Mods.Maps;
using UnityEditor;
using UnityEngine.SceneManagement;

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

            SetBasesCount(source, rawData);

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
                SceneBundlePath = source.SceneBundlePath,
                LilyBasesCount = source.LilyBasesCount,
                LotusBasesCount = source.LotusBasesCount,
                MultiTeamBasesCount = source.MultiTeamBasesCount,
                NeutralBasesCount = source.NeutralBasesCount
            };

            return mapData;
        }
        private void SetBasesCount(MapDataSO source, RawMapData dest)
        {
            if (source.BasesRoot != null)
            {
                var bases = source.BasesRoot.GetBasesCount();
                var keys = bases.Keys;

                foreach (var key in keys)
                {
                    switch (key)
                    {
                        case Teams.Lotus:
                            dest.LotusBasesCount = bases[key];
                            break;
                        case Teams.Lily:
                            dest.LilyBasesCount = bases[key];
                            break;
                        case Teams.Neutral:
                            dest.NeutralBasesCount = bases[key];
                            break;
                        case Teams.Multi:
                            dest.MultiTeamBasesCount = bases[key];
                            break;
                        default:
                            throw new ArgumentOutOfRangeException($"There is no such team as ${key}!");
                    }
                }
            }
            else
            {
                throw new GONBaseException("Bases root game object not specified!");
            }
        }
    }
}
