using UnityEngine;

namespace Assets.Scripts.Game.Player.Gravity.Constraints
{
    public interface IMaxVelConstraint
    {
        /// <summary>
        /// Checks if the constraint should be applied to provided velocity basing on player state.
        /// </summary>
        /// <param name="currentVelocity">Current velocity of the player.</param>
        /// <returns></returns>
        Vector2 ChkForConstraint(Vector2 currentVelocity);
    }
}
