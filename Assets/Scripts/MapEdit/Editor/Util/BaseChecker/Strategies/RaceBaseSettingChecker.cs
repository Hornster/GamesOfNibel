using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Data.Maps;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Helpers;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.MapEdit.Editor.Util.BaseChecker.Strategies
{
    /// <summary>
    /// Race mode requires a pair consisting of one start and one finish base for team Multi
    /// or one start and one finish base per team (Lotus and Lily). Neutral bases are not supported right now.
    /// </summary>
    public class RaceBaseSettingChecker : IBaseSettingChecker
    {
        private Dictionary<Teams, Dictionary<BaseTypeEnum, bool>> _setting;

        public bool ChkBasesSetting(List<BaseData> baseMarkers)
        {
            foreach (var baseMarker in baseMarkers)
            {
                if (baseMarker.GameMode != GameplayModesEnum.Race)
                {
                    continue;
                }

                ChkBase(baseMarker);
            }

            return ChkConditions();
        }

        private void ResetSetting()
        {
            var teams = EnumValueRetriever.GetEnumArray<Teams>();
            var baseTypes = EnumValueRetriever.GetEnumArray<BaseTypeEnum>();

            _setting = new Dictionary<Teams, Dictionary<BaseTypeEnum, bool>>();
            foreach (var team in teams)
            {
                if (team == Teams.Neutral)
                {
                    //Neutral bases are not supported in this mode right now.
                    continue;
                }

                var baseTypesDict = new Dictionary<BaseTypeEnum, bool>
                {
                    {BaseTypeEnum.RaceStart, false},
                    {BaseTypeEnum.RaceFinish, false}
                };

                _setting.Add(team, baseTypesDict);
            }
        }

        private void ChkBase(BaseData baseMarker)
        {
            var teamSetting = _setting[baseMarker.BaseTeam];
            if (teamSetting.ContainsKey(baseMarker.BaseType))
            {
                teamSetting[baseMarker.BaseType] = true;
            }
        }
        /// <summary>
        /// Checks if the configuration of the bases is correct for scenarios:
        /// ("allTypes" means all teams without Multi and Neutral)
        /// (Notation: StartBaseType -> FinishBaseType)
        ///
        /// Multi -> Multi
        /// allTypes -> Multi
        /// Multi -> allTypes
        /// allTypes -> allTypes
        ///
        /// At least one of the presented above configs has to be present for this
        /// method to return true. Will return false otherwise.
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ChkConditions()
        {
            var multiTeam = _setting[Teams.Multi];

            //Multi -> Multi scenario
            if (multiTeam[BaseTypeEnum.RaceStart] && multiTeam[BaseTypeEnum.RaceFinish])
            {
                return true;
            }

            //allTypes -> Multi scenario
            var lilyTeam = _setting[Teams.Lily];
            var lotusTeam = _setting[Teams.Lotus];

            if (multiTeam[BaseTypeEnum.RaceFinish])
            {
                if (lilyTeam[BaseTypeEnum.RaceStart] && lotusTeam[BaseTypeEnum.RaceStart])
                {
                    return true;
                }
            }

            //Multi -> allTypes scenario
            if (multiTeam[BaseTypeEnum.RaceStart])
            {
                if (lilyTeam[BaseTypeEnum.RaceFinish] && lotusTeam[BaseTypeEnum.RaceFinish])
                {
                    return true;
                }
            }

            //allTypes -> allTypes scenario
            if (lilyTeam[BaseTypeEnum.RaceStart] && lotusTeam[BaseTypeEnum.RaceStart])
            {
                if (lilyTeam[BaseTypeEnum.RaceFinish] && lotusTeam[BaseTypeEnum.RaceFinish])
                {
                    return true;
                }
            }

            ReportWrongSetting();

            return false;
        }

        private void ReportWrongSetting()
        {
            Debug.LogError($"Incorrect base setting for Race game mode! \n" +
                           $"Correct settings are:\n" +
                           $" Multi -> Multi\n" +
                           "allTypes -> Multi\n" +
                           "Multi -> allTypes\n" +
                           "allTypes -> allTypes\n\n" +
                           "Where \"allTypes\" refers to Lotus and Lily teams. Multi refers to Multi team.\n" +
                           $"Lotus and Lily teams must have a place to start the race ({BaseTypeEnum.RaceStart} base type)" +
                           $"and a place where the race ends ({BaseTypeEnum.RaceFinish} base type)." +
                           $"Multi team is sufficient for both Lotus and Lily.");
        }
    }
}

//TODO: Check if one of the conditions is fulfilled (allTeams means all without Multi and Neutral):
//TODO: Multi -> Multi
//TODO: Multi -> allTeams
//TODO: allTeams -> Multi
//TODO: allTeams -> allTeams
//TODO: If none is fulfilled - inform the player. Tell them current setting and show the correct ones as options.
//TODO: Let them decide how to modify what they currently have.