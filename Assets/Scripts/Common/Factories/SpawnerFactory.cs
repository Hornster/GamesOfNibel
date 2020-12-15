using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Data;
using Assets.Scripts.Common.Enums;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Common.Factories
{
    public class SpawnerFactory : MonoBehaviour, ISpawnerFactory
    {
        [Tooltip("Prefab for the neutral base variant.")]
        [SerializeField]
        private GameObject _neutralBaseVariant;
        [Tooltip("Prefab for the Lotus team base variant.")]
        [SerializeField]
        private GameObject _lotusBaseVariant;
        [Tooltip("Prefab for the Lily team base variant.")]
        [SerializeField]
        private GameObject _lilyBaseVariant;
        [Tooltip("Prefab for the Multi Team base variant.")]
        [SerializeField]
        private GameObject _multiTeamBaseVariant;

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
                    createdObject = Instantiate(_lotusBaseVariant);
                    break;
                case Teams.Lily:
                    createdObject = Instantiate(_lilyBaseVariant);
                    break;
                case Teams.Neutral:
                    createdObject = Instantiate(_neutralBaseVariant);
                    break;
                case Teams.Multi:
                    createdObject = Instantiate(_multiTeamBaseVariant);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }

            return createdObject;
        }
    }
}
