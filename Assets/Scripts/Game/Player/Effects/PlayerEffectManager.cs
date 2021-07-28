using System.Collections.Generic;
using Assets.Scripts.Game.Player.Data;
using UnityEngine;

namespace Assets.Scripts.Game.Player.Effects
{
    /// <summary>
    /// Manages the effects applied to the player.
    /// </summary>
    public class PlayerEffectManager : MonoBehaviour, IEffectsManager
    {
        /// <summary>
        /// Stores all active physical effects for the current frame.
        /// </summary>
        private List<IPlayerPhysicalEffect> _activeEffects = new List<IPlayerPhysicalEffect>();

        /// <summary>
        /// Applies the effects.
        /// </summary>
        /// <param name="playerRigidbody">The rigidbody of the player's character.</param>
        /// <param name="playerState">The state of the player that can be modified by the effect.</param>
        public void ApplyEffects(PlayerState playerState, Rigidbody2D playerRigidbody)
        {
            foreach (var effect in _activeEffects)
            {
                effect.InfluencePlayer(playerState, playerRigidbody);
            }

            _activeEffects.Clear();
        }
        /// <summary>
        /// Adds physical effect for current physics frame.
        /// </summary>
        /// <param name="effect"></param>
        public void AddPhysicalEffect(IPlayerPhysicalEffect effect)
        {
            _activeEffects.Add(effect);
        }
    }
}
