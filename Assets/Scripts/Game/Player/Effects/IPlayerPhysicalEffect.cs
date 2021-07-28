using Assets.Scripts.Game.Player.Data;
using UnityEngine;

namespace Assets.Scripts.Game.Player.Effects
{
    /// <summary>
    /// Defines classes that can affect player.
    /// </summary>
    public interface IPlayerPhysicalEffect
    {
        /// <summary>
        /// Directly influences the player's body.
        /// </summary>
        /// <param name="rb"></param>
        void InfluencePlayer(PlayerState playerState, Rigidbody2D rb);
    }
}
