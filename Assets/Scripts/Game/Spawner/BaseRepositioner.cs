using Assets.Scripts.Game.Common.Exceptions;
using Assets.Scripts.Game.Common.Helpers;
using Assets.Scripts.Game.Spawner.PlayerSpawner;
using UnityEngine;

namespace Assets.Scripts.Game.Spawner
{
    /// <summary>
    /// Allows for moving base around. Designed for load-time usage.
    /// </summary>
    public class BaseRepositioner : MonoBehaviour, IRepositioner, IRotator, IScaler
    {
        [Tooltip("The base's root gameobject.")]
        [SerializeField]
        private GameObject _baseRootGameObject;
        public void ChangePosition(Vector2 newPosition)
        {
            _baseRootGameObject.transform.position = newPosition;
            var playerRepositioner = _baseRootGameObject.GetComponentInChildren<PlayerPositioner>();

            if (playerRepositioner != null)
            {
                playerRepositioner.RepositionAllPlayers();
            }
        }

        public void Rotate(Quaternion rotation)
        {
            _baseRootGameObject.transform.rotation = rotation;
        }

        public void ChangeScale(Vector3 newScale)
        {
            _baseRootGameObject.transform.localScale = newScale;
        }
    }
}
