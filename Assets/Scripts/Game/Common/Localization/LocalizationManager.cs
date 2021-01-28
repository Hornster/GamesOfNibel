namespace Assets.Scripts.Game.Common.Localization
{
    /// <summary>
    /// Provides messages and translations.
    /// </summary>
    public class LocalizationManager
    {
        private static LocalizationManager _instance;
        public CtfGameModeMessages CtfLocale { get; }

        /// <summary>
        /// Returns the instance of this singleton.
        /// </summary>
        /// <returns></returns>
        public static LocalizationManager GetInstance()
        {
            return _instance ?? (_instance = new LocalizationManager());
        }
        //Singleton constructor is private.
        private LocalizationManager()
        {
            CtfLocale = new CtfGameModeMessages();
        }
    }
}
