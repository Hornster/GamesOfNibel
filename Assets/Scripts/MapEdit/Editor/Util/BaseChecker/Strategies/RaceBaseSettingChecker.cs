using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool ChkBasesSetting(List<BaseMarkerData> baseMarkers)
        {
            foreach (var baseMarker in baseMarkers)
            {
                if (baseMarker.GameMode != GameplayModesEnum.Race)
                {
                    continue;
                }

                ChkBase(baseMarker);
            }

            throw new NotImplementedException();
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

        private void ChkBase(BaseMarkerData baseMarker)
        {
            var teamSetting = _setting[baseMarker.BaseTeam];
            if (teamSetting.ContainsKey(baseMarker.BaseType))
            {
                teamSetting[baseMarker.BaseType] = true;
            }
        }

        private void ChkConditions()
        {
            var multiTeam = _setting[Teams.Multi];

        }

        private void ReportWrongSetting(Teams team, BaseTypeEnum baseType)
        {
            switch(baseType)
            {
                case BaseTypeEnum.RaceStart:
                case BaseTypeEnum.RaceFinish:
                    ReportLackOfBase(team, baseType);
                    break;
                default:
                    Debug.LogWarning($"{baseType} is wrong base type for Race mode (team {team}).");
                    break;
            }
        }

        private void ReportLackOfBase(Teams team, BaseTypeEnum lackingBaseType)
        {
            Debug.LogError($"Team {team} lacks base type {lackingBaseType}.");
        }
    }
}

//TODO: Check if one of the conditions is fulfilled (allTeams means all without Multi):
//TODO: Multi -> Multi
//TODO: Multi -> allTeams
//TODO: allTeams -> Multi
//TODO: allTeams -> allTeams
//TODO: If none is fulfilled - inform the player. Tell them current setting and show the correct ones as options.
//TODO: Let them decide how to modify what they currently have.