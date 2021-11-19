using UnityEngine;

namespace Assets.Scripts.Game.Common.Data.Constants
{
    /// <summary>
    /// Spirit Games constants. SO means Scriptable Object.
    /// </summary>
    public static class SGConstants
    {
        public const string SGSOMenuPath = "ScriptableObjects/SG/";
        public const string SGSOMapSelectionRelativePath = "Map Selection/";
        public const string SGSOSceneChangingRelativePath = "Scene Changing/";
        public const string SGSOTransitionsRelativePath = "Transitions/";
        public const string SGSOColorsRelativePath = "Colors/";
        public const string SGSOFactoryDataRelativePath = "Factory Data/";

        public const string SGPlayerDefaultName = "RANDOM SPIRIT";
        /// <summary>
        /// Shown when the round of race game mode ends. Formatting args:
        /// 1: Name of the player - string
        /// 2: Time the player finished race in - string.
        /// </summary>
        public const string SGRaceFinishMessage = "{0} has won with time {1}!";
        public const string SGRaceTimerFormat = @"hh\:mm\:ss\.fff";
        /// <summary>
        /// The resolution factor: width/height.
        /// </summary>
        public const double ReferenceResolutionFactor = 16.0 / 9.0;
        public const int ReferenceResolutionX = 3840;
        public const int ReferenceResolutionY = 2160;
    }
}
