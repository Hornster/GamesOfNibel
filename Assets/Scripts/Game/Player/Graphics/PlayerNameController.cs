using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.Data;
using Assets.Scripts.Game.Common.Data.Constants;
using Assets.Scripts.Game.Player.Data;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Game.Player.Graphics
{
    public class PlayerNameController : MonoBehaviour
    {
        [Tooltip("The control used to show the player's name.")]
        [SerializeField]
        private TMP_Text _playerNameControl;

        [Tooltip("name of the player visible above them.")] 
        [SerializeField]
        private PlayerMatchData _playerMatchData;
        

        private void Start()
        {
            _playerNameControl.text = _playerMatchData.PlayerName;
        }
    }
}
