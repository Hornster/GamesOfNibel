using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.Localization
{
    /// <summary>
    /// Provides messages and translations.
    /// </summary>
    public class LocalizationManager
    {
        private static LocalizationManager _instance;
        private CtfGameModeMessages CtfLocale { get; }

        /// <summary>
        /// Returns the instance of this singleton.
        /// </summary>
        /// <returns></returns>
        public LocalizationManager GetInstance()
        {
            return _instance ?? (_instance = new LocalizationManager());
        }
        //Singleton constructor is private.
        private LocalizationManager(){}
    }
}
