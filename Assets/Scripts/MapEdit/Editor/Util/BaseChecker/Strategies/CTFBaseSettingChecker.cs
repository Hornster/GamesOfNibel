using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.MapEdit.Editor.Util.BaseChecker.Strategies
{
    public class CTFBaseSettingChecker : IBaseSettingChecker
    {
        private bool _hasLilyBase;
        private bool _hasLotusBase;
        private bool _hasNeutralBase;
        public bool ChkBasesSetting(List<BaseMarkerData> baseMarkers)
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
                        Debug.LogWarning("Multi-team bases do not apply to Capture The Flag mode. You can leave the marker if you want, it will not cause any problems but it won't be used either.");
                        break;
                    default:
                        throw new Exception(baseMarker.BaseTeam + Errors.TeamNotFound);
                }
            }

            return _hasLilyBase && _hasLotusBase && _hasNeutralBase;
        }
    }
}
