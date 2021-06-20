using Assets.Scripts.Game.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.MapEdit.Editor.Data.Constants
{
    public class SGMapEditMessages
    {
        public static readonly string RaceBaseSettingIncorrectErrMessage = $"Incorrect base setting for Race game mode! \n" +
                           $"Correct settings are:\n" +
                           $" Multi -> Multi\n" +
                           "allTypes -> Multi\n" +
                           "Multi -> allTypes\n" +
                           "allTypes -> allTypes\n\n" +
                           "Where \"allTypes\" refers to Lotus and Lily teams. Multi refers to Multi team.\n" +
                           $"Lotus and Lily teams must have a place to start the race ({BaseTypeEnum.RaceStart} base type)" +
                           $"and a place where the race ends ({BaseTypeEnum.RaceFinish} base type)." +
                           $"Multi team is sufficient for both Lotus and Lily.";

        public static readonly string MapCreationErrorBadBaseSetting = "Map creating process interrupted. Check error messages.";
        public static readonly string GlobalGravityManagerInfo = "You can modify these values if you want, though make sure you know what are you doing. Default values correspond to Ori games as close as I was able to measure them. Negative and equal to 0 values can result in undefined behavior - set these at your own risk.";
    }
}
