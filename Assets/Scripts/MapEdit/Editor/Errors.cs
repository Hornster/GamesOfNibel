using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.MapEdit.Editor
{
    public class Errors
    {
        #region Exceptions
        /// <summary>
        /// {0} - Team name
        /// </summary>
        public const string TeamNotFoundException = "{0} team not recognized.";

        /// <summary>
        /// {0} - asset name
        /// </summary>
        public const string AssetSeekerAssetNotFoundGlobally = "Cannot find {0} globally! Check the installation package for the asset.";
        /// <summary>
        /// {0} - asset name
        /// </summary>
        public const string AssetSeekerMultipleAssetsFoundGlobally = "Only one {0} is allowed! Found more than one globally!";
        /// <summary>
        /// {0} - asset name
        /// {1} - path to the asset 
        /// </summary>
        public const string AssetSeekerMultipleAssetsFoundAtPath = "Only one {0} is allowed at {1}!";
        #endregion

        #region LogErrors

        #endregion

        #region LogWarnings
        public const string CTFMultiTeamBaseFoundWarning =
            "Multi-team bases do not apply to Capture The Flag mode. You can leave the marker if you want, it will not cause any problems but it won't be used either.";

        /// <summary>
        /// {0} - asset name
        /// {1} - path to the asset 
        /// </summary>
        public const string AssetSeekerAssetNotFoundAtPath = "Cannot find {0} at {1}! Trying more broad search...";

        #endregion

        #region LogInfos

        #endregion
    }
}
