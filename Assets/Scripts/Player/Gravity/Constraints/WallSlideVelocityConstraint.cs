using UnityEngine;

namespace Assets.Scripts.Player.Gravity.Constraints
{
    public class WallSlideVelocityConstraint : IMaxVelConstraint
    {
        /// <summary>
        /// Maximal falling velocity allowed. Falling means negative value.
        /// </summary>
        private float _maxFallingVelocityY;

        private PlayerState _playerState;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerState">State of the player.</param>
        /// <param name="maxFallingVelocityY">Maximal falling velocity allowed. Falling means negative value.</param>
        public WallSlideVelocityConstraint(PlayerState playerState, float maxFallingVelocityY)
        {
            _maxFallingVelocityY = -maxFallingVelocityY;
            _playerState = playerState;
        }

        public Vector2 ChkForConstraint(Vector2 currentVelocity)
        {
            if (_playerState.IsTouchingWall && !_playerState.isGrounded)
            {
                if (currentVelocity.y < _maxFallingVelocityY)
                {   //If we are here - the velocity was greater than the constraint allows for.
                    //Override it.
                    currentVelocity.y = _maxFallingVelocityY;
                }
            }

            return currentVelocity;
        }
    }
}
