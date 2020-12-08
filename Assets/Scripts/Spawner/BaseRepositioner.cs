using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Exceptions;
using Assets.Scripts.Common.Helpers;
using Assets.Scripts.Spawner.PlayerSpawner;
using UnityEngine;

namespace Assets.Scripts.Spawner
{
    /// <summary>
    /// Allows for moving base around. Designed for load-time usage.
    /// </summary>
    public class BaseRepositioner : MonoBehaviour, IRepositioner
    {
        [Tooltip("The base's root gameobject.")]
        [SerializeField]
        private GameObject _baseRootGameObject;
        public void ChangePosition(Vector2 newPosition)
        {
            _baseRootGameObject.transform.position = newPosition;
            var playerRepositioner = _baseRootGameObject.GetComponentInChildren<PlayerPositioner>();

            if (playerRepositioner == null)
            {
                throw new GONBaseException("Tried to move players on object that is not a valid player spawning base!");
            }

            playerRepositioner.RepositionAllPlayers();
        }
    }
}
