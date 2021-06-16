using System.Collections.Generic;
using Assets.Scripts.Game.Common.Data.Maps;

namespace Assets.Scripts.MapEdit.Editor.Util.BaseChecker
{
    public interface IBaseSettingChecker
    {
        bool ChkBasesSetting(List<BaseData> baseMarkers);
    }
}
