using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Common.Data
{
    /// <summary>
    /// Stores config for all spawners for given team.
    /// </summary>
    public class SpawnerGroupConfig : MonoBehaviour
    {
        [Tooltip("Team this spawner belongs to.")]
        [SerializeField]
        private Teams _myTeam;
        [Tooltip("How many spawns of this type should be created for the map.")]
        [SerializeField]
        private int _quantity;
        /// <summary>
        /// What team does the spawn group belong to.
        /// </summary>
        public Teams MyTeam { get => _myTeam; set => _myTeam = value; }
        /// <summary>
        /// How many spawns of this team there should be.
        /// </summary>
        public int Quantity { get => _quantity; set => _quantity=value; }
    }
}
