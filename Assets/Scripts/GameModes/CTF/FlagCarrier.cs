using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.GameModes.CTF
{
    /// <summary>
    /// Allows the given gameobject to be able to carry flags.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class FlagCarrier : MonoBehaviour, IFlagCarrier
    {
        [SerializeField]
        private Teams _myTeam;

        public Teams MyTeam => _myTeam;
    }
}
