using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Data.Maps;
using Assets.Scripts.Game.Common.Enums;
using Assets.Scripts.Game.Common.Helpers;
using Assets.Scripts.MapEdit.Editor.Util.BaseChecker;
using Assets.Scripts.MapEdit.Editor.Util.BaseChecker.Strategies;
using UnityEngine;

namespace Assets.Scripts.MapEdit.Editor.Util
{
    public class BaseSettingChecker
    {
        private IBaseSettingChecker _usedChecker;
        public bool ChkBasesSetting(List<BaseData> baseMarkers)
        {
            var modeCheck = new Dictionary<GameplayModesEnum, List<BaseData>>();

            foreach (var marker in baseMarkers)
            {
                if (modeCheck.TryGetValue(marker.GameMode, out var alreadyAssignedBases))
                {
                    alreadyAssignedBases.Add(marker);
                }
                else
                {
                    modeCheck.Add(marker.GameMode, new List<BaseData>(){marker});
                }
            }

            return ChkAgainstBaseSetting(modeCheck);
        }
        private bool ChkAgainstBaseSetting(Dictionary<GameplayModesEnum, List<BaseData>> baseMarkers)
        {
            var modes = EnumValueRetriever.GetEnumArray<GameplayModesEnum>();

            foreach (var mode in modes)
            {
                switch(mode)
                {
                    case GameplayModesEnum.CTF:
                        _usedChecker = new CTFBaseSettingChecker();
                        return _usedChecker.ChkBasesSetting(baseMarkers[GameplayModesEnum.CTF]);
                        break;
                    case GameplayModesEnum.Race:
                        _usedChecker = new RaceBaseSettingChecker();
                        return _usedChecker.ChkBasesSetting(baseMarkers[GameplayModesEnum.Race]);
                        break;
                    case GameplayModesEnum.TimeAttack:
                        throw new NotImplementedException();
                        break;
                    default:
                        Debug.LogWarning($"The {mode} gamemode check is not supported. Make sure you have the latest version of editor package installed.");
                        return false;
                }
            }

            Debug.LogError("No bases detected!");
            return false;
        }
        private void ChkAgainstTeams(List<BaseData> ctfMarkers)
        {

        }
        
    }
}
