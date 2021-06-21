using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Data.Maps;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.MapEdit.Editor.Util.BaseChecker.Strategies
{
    public class CTFBaseSettingChecker : IBaseSettingChecker
    {
        private bool _hasLilyBase;
        private bool _hasLotusBase;
        private bool _hasNeutralBase;
        public bool ChkBasesSetting(List<BaseData> baseMarkers)
        {
            _hasLilyBase = false;
            _hasLotusBase = false;
            _hasNeutralBase = false;

            foreach (var baseMarker in baseMarkers)
            {
                if (baseMarker.GameMode != GameplayModesEnum.CTF)
                {
                    continue;
                }
                switch(baseMarker.BaseTeam)
                {
                    case Teams.Lotus:
                        _hasLotusBase = true;
                        break;
                    case Teams.Lily:
                        _hasLilyBase = true;
                        break;
                    case Teams.Neutral:
                        _hasNeutralBase = true;
                        break;
                    case Teams.Multi:
                        Debug.LogWarning(Errors.CTFMultiTeamBaseFoundWarning);
                        break;
                    default:
                        throw new Exception(string.Format(Errors.TeamNotFoundException, baseMarker.BaseTeam));
                }
            }

            return _hasLilyBase && _hasLotusBase && _hasNeutralBase;
        }
    }
}
