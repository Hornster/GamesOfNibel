using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.Enums
{
    /// <summary>
    /// Defines glide skill stages.
    /// </summary>
    public enum GlideStages
    {
        /// <summary>
        /// When the user presses the button responsible for using glide ability.
        /// </summary>
        GlideBegin,
        /// <summary>
        /// The user keeps the glide button pressed.
        /// </summary>
        GlideKeep,
        /// <summary>
        /// The user releases the glide button.
        /// </summary>
        GlideStop
    }
}
