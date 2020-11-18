using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Data.NoDestroyOnLoad;
using UnityEngine;

namespace Assets.Scripts.Common.Factories
{
    /// <summary>
    /// Creates characters.
    /// </summary>
    public interface ICharacterFactory
    {
        /// <summary>
        /// Create character based on provided config.
        /// </summary>
        /// <param name="playerConfig">Instance of character configuration.</param>
        /// <returns>GameObject of freshly created character.</returns>
        GameObject CreateCharacter(PlayerConfig playerConfig);
    }
}
