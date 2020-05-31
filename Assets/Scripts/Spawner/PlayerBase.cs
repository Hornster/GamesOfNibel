
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Spawner
{
    public class PlayerBase : MonoBehaviour
    {
        /// <summary>
        /// The team to which this base belongs.
        /// </summary>
        [SerializeField]
        private Teams _myTeam;
        /// <summary>
        /// Spawner assigned to this base.
        /// </summary>
        private ISpawner _mySpawner;

        public void SpawnEntity()
        {
            _mySpawner.SpawnEntity(_myTeam);
        }
        //TODO this won't do. Try to diverge player and flag spawners on higher abstraction level, like an interface.
    }
}
