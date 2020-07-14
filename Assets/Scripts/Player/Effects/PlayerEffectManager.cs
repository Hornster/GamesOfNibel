using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.Effects
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
        public void ApplyEffects(Rigidbody2D playerRigidbody)
        {
            foreach (var effect in _activeEffects)
            {
                effect.InfluencePlayer(playerRigidbody);
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
