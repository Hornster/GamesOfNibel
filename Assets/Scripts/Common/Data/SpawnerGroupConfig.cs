using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Common.Data
{
    public class SpawnerGroupConfig : MonoBehaviour
    {
        /// <summary>
        /// What team does the spawn group belong to.
        /// </summary>
        public Teams MyTeam { get; set; }
        /// <summary>
        /// How many spawns of this team there should be.
        /// </summary>
        public int Quantity { get; set; }
    }
}
