using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common;
using Assets.Scripts.Game.Common.Data.Constants;
using Assets.Scripts.Game.Common.Enums;
using UnityEngine;

namespace Assets.Scripts.Game.Player.Data
{
    /// <summary>
    /// Common data of the player that is used by various outside objects during match.
    /// </summary>
    public class PlayerMatchData : MonoBehaviour
    {
        [SerializeField]
        private TeamModule _playerTeam;
        /// <summary>
        /// Unique ID of the player.
        /// </summary>
        public int PlayerID { get; set; }
        public Teams PlayerTeam => _playerTeam.MyTeam;

        /// <summary>
        /// Name of the player visible in the match.
        /// </summary>
        public string PlayerName { get; set; } = SGConstants.SGPlayerDefaultName;
    }
}
