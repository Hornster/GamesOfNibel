using Assets.Scripts.Game.Player.Data;
using UnityEngine;

namespace Assets.Scripts.Game.Player.Gravity.Constraints
{
    public class GlideVelocityConstraint : IMaxVelConstraint
    {
        /// <summary>
        /// Max falling velocity Y allowed when gliding.
        /// Falling means negative values.
        /// </summary>
        private float _maxFallingVelocity;
        /// <summary>
        /// Contains data about the player.
        /// </summary>
        private PlayerState _playerState;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerState">State of the player.</param>
        /// <param name="maxFallingVelocity">Max falling velocity Y allowed when gliding. Falling means negative values.</param>
        public GlideVelocityConstraint(PlayerState playerState, float maxFallingVelocity)
        {
            _maxFallingVelocity = -maxFallingVelocity;
            _playerState = playerState;
        }

        public Vector2 ChkForConstraint(Vector2 currentVelocity)
        {
            if (_playerState.IsGliding)
            {
                if (currentVelocity.y < _maxFallingVelocity)
                {
                    currentVelocity.y = _maxFallingVelocity;
                }
            }

            return currentVelocity;
        }
    }
}
