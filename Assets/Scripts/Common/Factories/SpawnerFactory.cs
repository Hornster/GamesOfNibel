using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Data;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Common.Factories
{
    public class SpawnerFactory : MonoBehaviour, ISpawnerFactory
    {
        [Tooltip("Prefab for the neutral spawn variant.")]
        [SerializeField]
        private GameObject _neutralSpawnerVariant;
        [Tooltip("Prefab for the Lotus team spawn variant.")]
        [SerializeField]
        private GameObject _lotusSpawnerVariant;
        [Tooltip("Prefab for the Lily team spawn variant.")]
        [SerializeField]
        private GameObject _lilySpawnerVariant;
        /// <summary>
        /// Dictates how many spawners of single group can be created at once.
        /// </summary>
        private const int SpawnersInSingleGroupCount = 25;
        public List<GameObject>CreateSpawner(SpawnerGroupConfig spawnerGroupConfig)
        {
            if (spawnerGroupConfig.Quantity > SpawnersInSingleGroupCount)
            {
                spawnerGroupConfig.Quantity = SpawnersInSingleGroupCount;
            }

            var spawnersBatch = new List<GameObject>(spawnerGroupConfig.Quantity);

            for (int i = 0; i < spawnerGroupConfig.Quantity; i++)
            {
                spawnersBatch.Add(CreateSpawn(spawnerGroupConfig.MyTeam));
            }

            return spawnersBatch;
        }

        private GameObject CreateSpawn(Teams team)
        {
            GameObject createdObject;

            switch (team)
            {
                case Teams.Lotus:
                    createdObject = Instantiate(_lotusSpawnerVariant);
                    break;
                case Teams.Lily:
                    createdObject = Instantiate(_lilySpawnerVariant);
                    break;
                case Teams.Neutral:
                case Teams.Multi:
                    createdObject = Instantiate(_neutralSpawnerVariant);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }

            return createdObject;
        }
    }
}
