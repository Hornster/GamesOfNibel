using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common.Enums;
using Assets.Scripts.GameModes.CTF.Entities;
using UnityEngine;

namespace Assets.Scripts.GameModes.CTF
{
    /// <summary>
    /// Allows the player  object to carry flags.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class CharacterFlagCarrier : MonoBehaviour, IFlagCarrier
    {
        /// <summary>
        /// Remembers the flag that is currently being carried. When is not
        /// carrying any flag - is null.
        /// </summary>
        private IFlag _carriedFlag;
        [SerializeField]
        private Teams _myTeam;
        public Teams MyTeam => _myTeam;

        public Transform MyTransform => gameObject.transform;
        public bool HasFlag { get; private set; }

        //TODO Add taking over the flag from players that run next to yuo and have the flag with them.
        /// <summary>
        /// Returns the carried flag. If no flag - returns null.
        /// Worth checking the HasFlag first.
        /// </summary>
        /// <returns></returns>
        public IFlag TakeOverFlag()
        {
            HasFlag = false;
            var carriedFlag = _carriedFlag;
            _carriedFlag = null;

            return carriedFlag;
        }
    }
}
