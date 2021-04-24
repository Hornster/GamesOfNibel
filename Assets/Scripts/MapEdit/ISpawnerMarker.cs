using Assets.Scripts.Game.Common;
using UnityEngine;

namespace Assets.Scripts.MapEdit
{
    /// <summary>
    /// Interface used for finding spawner positions on maps (so far, might change later).
    /// </summary>
    public interface ISpawnerMarker
    {
        /// <summary>
        /// Transform which determines where the future spawn should be put.
        /// </summary>
        Transform MarkerTransform { get; }
        /// <summary>
        /// Returns object with info about the spawner's team.
        /// </summary>
        TeamModule SpawnerTeam { get; }

        /// <summary>
        /// Move the spawn to the location of this marker.
        /// </summary>
        /// <param name="spawnGameObject">The main gameobject of the spawn that should be moved.</param>
        /// <returns>TRUE when spawn has been successfully assigned. FALSE if there was a spawn already assigned beforehand.
        /// When FALSE is returned, passed spawn is not moved.</returns>
        bool MoveSpawn(GameObject spawnGameObject);
    }
}
