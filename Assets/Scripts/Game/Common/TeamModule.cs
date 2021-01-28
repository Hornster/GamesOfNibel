using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Common
{
    /// <summary>
    /// Puts the team definition in one place.
    /// </summary>
    public class TeamModule : MonoBehaviour
    {
        [SerializeField]
        private Teams _myTeam;

        /// <summary>
        /// The team of the entity this module was assigned to.
        /// </summary>
        public Teams MyTeam
        {
            get => _myTeam;
            set => _myTeam = value;
        }

    }
}
