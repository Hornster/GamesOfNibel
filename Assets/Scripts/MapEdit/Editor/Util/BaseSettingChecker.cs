using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void ChkBasesSetting(List<BaseMarkerData> baseMarkers)
        {
            var modeCheck = new Dictionary<GameplayModesEnum, List<BaseMarkerData>>();

            foreach (var marker in baseMarkers)
            {
                if (modeCheck.TryGetValue(marker.GameMode, out var alreadyAssignedBases))
                {
                    alreadyAssignedBases.Add(marker);
                }
                else
                {
                    modeCheck.Add(marker.GameMode, new List<BaseMarkerData>(){marker});
                }
            }
        }
        private void ChkAgainstBaseSetting(Dictionary<GameplayModesEnum, List<BaseMarkerData>> baseMarkers)
        {
            var modes = EnumValueRetriever.GetEnumArray<GameplayModesEnum>();

            foreach (var mode in modes)
            {
                switch(mode)
                {
                    case GameplayModesEnum.CTF:
                        break;
                    case GameplayModesEnum.Race:
                        break;
                    case GameplayModesEnum.TimeAttack:
                        break;
                    default:
                        Debug.LogWarning($"The {mode} gamemode check is not supported. Make sure you have the latest version of editor package installed.");
                        break;
                }
            }
        }
        private void ChkAgainstTeams(List<BaseMarkerData> ctfMarkers)
        {

        }

        private bool ChkAgainstCTF(List<BaseMarkerData> ctfMarkers)
        {
            bool hasMultiBase = false;
            bool hasNeutralBase = false;
            bool hasLotusBase = false;
            bool hasLilyBase = false;

        }

       

        
    }
}
