using Assets.Scripts.Game.Common.Data.NoDestroyOnLoad;
using UnityEngine;

namespace Assets.Scripts.Game.Common.Factories
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
