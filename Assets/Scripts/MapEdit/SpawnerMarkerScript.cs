using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.Helpers;
using UnityEngine;

namespace Assets.Scripts.MapEdit
{
    /// <summary>
    /// Script solely used for finding spawner positions on maps (so far, might change later).
    /// </summary>
    [RequireComponent(typeof(TeamModule))]
    public class SpawnerMarkerScript : MonoBehaviour, ISpawnerMarker
    {
        [Tooltip("Object defining the team of the marker, and thus, its future spawn counterpart.")]
        [SerializeField]
        private TeamModule _spawnerTeam;
        /// <summary>
        /// Returns the gameobject this script is attached to.
        /// </summary>
        public Transform MarkerTransform => transform;

        public TeamModule SpawnerTeam => _spawnerTeam;

        /// <summary>
        /// Set to true when the marker has a spawn assigned.
        /// </summary>
        public bool HasSpawnAssigned { get; private set; }

        
        /// <summary>
        /// Move the spawn to the location of this marker.
        /// </summary>
        /// <param name="spawnGameObject">The main gameobject of the spawn that should be moved.</param>
        /// <returns>TRUE when spawn has been successfully assigned. FALSE if there was a spawn already assigned beforehand.
        /// When FALSE is returned, passed spawn is not moved.</returns>
        public bool MoveSpawn(GameObject spawnGameObject)
        {
            if (HasSpawnAssigned)
            {
                return false;
            }

            var baseRepositioner = spawnGameObject.GetComponentInChildren<IRepositioner>();
            baseRepositioner.ChangePosition(transform.position);

            var baseRotator = spawnGameObject.GetComponentInChildren<IRotator>();
            baseRotator.Rotate(transform.rotation);

            //var destinationPos = this.gameObject.transform.position;
            //spawnGameObject.transform.position = destinationPos;

            HasSpawnAssigned = true;
            return HasSpawnAssigned;
        }

    }
}
