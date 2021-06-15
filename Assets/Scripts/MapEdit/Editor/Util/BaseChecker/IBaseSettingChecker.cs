using System.Collections.Generic;

namespace Assets.Scripts.MapEdit.Editor.Util.BaseChecker
{
    public interface IBaseSettingChecker
    {
        bool ChkBasesSetting(List<BaseMarkerData> baseMarkers);
    }
}
