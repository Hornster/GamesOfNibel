using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Game.Common.CustomEvents;
using Assets.Scripts.Game.Player;
using UnityEngine;

namespace Assets.Scripts.Game.GameModes.Race
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class RaceBaseController : MonoBehaviour
    {
        private IntegerUnityEvent _playerArrived;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            

        }

        private void ChkForPlayer(Collider2D collider)
        {
            var playerController = collider.GetComponent<PlayerController>();
            if (playerController == null)
            {
                return;
            }


        }
    }
}
