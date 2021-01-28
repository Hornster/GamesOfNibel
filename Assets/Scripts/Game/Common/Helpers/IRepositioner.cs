using UnityEngine;

namespace Assets.Scripts.Game.Common.Helpers
{
    /// <summary>
    /// Used to reposition objects.
    /// </summary>
    public interface IRepositioner
    {
        /// <summary>
        /// Causes object to be moved to provided position.
        /// </summary>
        /// <param name="newPosition">New position of the object.</param>
        void ChangePosition(Vector2 newPosition);
    }
}
